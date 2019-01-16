using DeploymentTool.API.Settings;
using DeploymentTool.Core.Filesystem;
using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeploymentTool.API.Services
{
    public static class BackupService
    {
        public static void CreateBackup(DeploySession session, FilesystemDifference difference)
        {
            var profile = ProfileService.GetProfileById(session.ProfileId);
            if (difference.CreatedFiles.Count +
                difference.ModifiedFiles.Count +
                difference.RemovedFiles.Count == 0)
            {
                return;
            }

            var backupFolderPath = InitBackupDirectory(session);
            var profilePath = FilesystemUtils.NormalizePath(profile.RootFolder);
            var logFileName = Path.Combine(backupFolderPath, "Log.txt");
            var revertFileName = Path.Combine(backupFolderPath, "rollback.dat");

            try
            {
                using (var logWriter = new StreamWriter(logFileName))
                {
                    logWriter.WriteLine("Backup Initialized: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    logWriter.WriteLine("CreatedFiles: " + difference.CreatedFiles.Count);
                    logWriter.WriteLine("ModifiedFiles: " + difference.ModifiedFiles.Count);
                    logWriter.WriteLine("RemovedFiles: " + difference.RemovedFiles.Count);

                    GenerateRollbackScript(revertFileName, difference);

                    foreach (var item in difference.RemovedFiles.Union(difference.ModifiedFiles))
                    {
                        var filePath = Path.Combine(profile.RootFolder, item.Filename);
                        if (File.Exists(filePath))
                        {
                            var filename = Path.GetFileName(filePath);
                            var fileRelativeDirectory = Path.GetDirectoryName(item.Filename);
                            var backupFileDirectory = Path.Combine(backupFolderPath, fileRelativeDirectory);
                            var targetFilePath = Path.Combine(backupFileDirectory, filename);

                            FilesystemUtils.CreateDirectory(backupFileDirectory);
                            try
                            {
                                File.Copy(filePath, targetFilePath);
                            }
                            catch (Exception e)
                            {
                                var error = $"Errors occured when attempt to backup file {filename} form {filePath} to {targetFilePath}\r\n exception: {e}";
                                logWriter.Write(error);
                                throw new Exception(error);
                            }
                        }
                        else
                        {
                            var error = $"Can not find file {filePath}";
                            logWriter.Write(error);
                            throw new Exception(error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Critical exception has occured: {ex}");
            }
        }

        public static void GenerateRollbackScript(string backupFolder, FilesystemDifference difference)
        {
            var revertFileName = Path.Combine(backupFolder, "rollback.dat");
            using (var revertWriter = new StreamWriter(revertFileName))
            {

                revertWriter.WriteLine("#CreatedFiles#");
                revertWriter.WriteLine(string.Join(Environment.NewLine, difference.CreatedFiles.Select(x => x.Filename)));
                revertWriter.WriteLine();


                revertWriter.WriteLine("#ModifiedFiles#");
                revertWriter.WriteLine(string.Join(Environment.NewLine, difference.ModifiedFiles.Select(x => x.Filename)));
                revertWriter.WriteLine();


                revertWriter.WriteLine("#RemovedFiles#");
                revertWriter.WriteLine(string.Join(Environment.NewLine, difference.RemovedFiles.Select(x => x.Filename)));
            }
        }

        public static string InitBackupDirectory(DeploySession session)
        {
            var targetFolder = GetSessionFullBackupDirectory(session);
            Directory.CreateDirectory(targetFolder);
            return targetFolder;
        }

        public static string GetSessionFullBackupDirectory(DeploySession session)
        {
            string targetFolder = Path.Combine(SettingsManager.Instance.BackupsFolder, session.GetDirectoryName());
            return targetFolder;
        }

        public static void Rollback(DeploySession session)
        {
            var backupDir = GetSessionFullBackupDirectory(session);
            if (!Directory.Exists(backupDir))
            {
                throw new DirectoryNotFoundException(backupDir);
            }

            var sessionDirectoryName = session.GetDirectoryName();

            ServerProfile profile = ProfileService.GetProfileById(session.ProfileId);

            string targetFolder = profile?.RootFolder;
            string sourceFolder = Path.Combine(SettingsManager.Instance.BackupsFolder, sessionDirectoryName);
            string backupFolder = Path.Combine(SettingsManager.Instance.DeploySessionFolder, sessionDirectoryName);

            if (profile == null)
            {
                throw new Exception("Profile not valid");
            }

            if (!Directory.Exists(sourceFolder))
            {
                throw new DirectoryNotFoundException("There is no directory with deploying files: " + sourceFolder);
            }

            if (!Directory.Exists(targetFolder))
            {
                throw new DirectoryNotFoundException("There is no destination directory: " + targetFolder);
            }

            var rollbackFileName = Path.Combine(backupDir, "rollback.dat");
            if (!File.Exists(rollbackFileName))
            {
                throw new FileNotFoundException("There is no directory with deploying files: " + rollbackFileName);
            }

            var rollbackLines = File.ReadAllLines(rollbackFileName);
            var difference = GetRollbackDifference(rollbackLines);

            //if some error will be occuerd we need to rollback
            //so we write rollback script only for already processed files
            //this variable will be changed in BackupService.MoveFiles method
            FilesystemDifference realDifference = new FilesystemDifference();

            //revert filesystem state that was before deployment
            MoveFiles(sourceFolder, targetFolder, backupFolder, difference, realDifference);
        }

        private static FilesystemDifference GetRollbackDifference(string[] rollbackLines)
        {
            FilesystemDifference difference = new FilesystemDifference();
            List<FileDataModel> currentList = null;
            for (int i = 0; i < rollbackLines.Length; i++)
            {
                var line = rollbackLines[i].Trim();
                if (!string.IsNullOrEmpty(line))
                {
                    switch (line)
                    {
                        case "#CreatedFiles#":
                            currentList = difference.RemovedFiles;
                            continue;
                        case "#ModifiedFiles#":
                            currentList = difference.ModifiedFiles;
                            continue;
                        case "#RemovedFiles#":
                            currentList = difference.CreatedFiles;
                            continue;
                    }

                    currentList?.Add(new FileDataModel()
                    {
                        Filename = line
                    });
                }
            }

            return difference;
        }

        /// <summary>
        /// Moves all files listed in difference 
        /// </summary>
        /// <param name="sourceFolder">Folder where stored prepared files</param>
        /// <param name="targetFolder">Live site folder</param>
        /// <param name="backupFolder">Replaced and deleted files from live will be moved here</param>
        /// <param name="difference">Object that listed which files must be processed</param>
        /// <param name="doneDifference">Output variable that listed files that have been already processed 
        /// (if method throws exception here will be files that was moved before this exception was thrown)</param>
        public static void MoveFiles(string sourceFolder, string targetFolder, string backupFolder, FilesystemDifference difference, FilesystemDifference doneDifference)
        {
            var filesToDeploy = difference.CreatedFiles.Union(difference.ModifiedFiles);

            foreach (var item in filesToDeploy)
            {
                var sourceFilename = Path.Combine(sourceFolder, item.Filename);
                var targetFilename = Path.Combine(targetFolder, item.Filename);
                var backupFilename = Path.Combine(backupFolder, item.Filename);

                if (File.Exists(targetFilename))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(backupFilename));
                    File.Replace(sourceFilename, targetFilename, backupFilename);
                    doneDifference.ModifiedFiles.Add(item);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(targetFilename));
                    File.Move(sourceFilename, targetFilename);
                    doneDifference.CreatedFiles.Add(item);
                }
            }

            foreach (var item in difference.RemovedFiles)
            {
                var targetFilename = Path.Combine(targetFolder, item.Filename);
                var backupFilename = Path.Combine(backupFolder, item.Filename);
                Directory.CreateDirectory(Path.GetDirectoryName(targetFilename));
                File.Move(targetFilename, backupFilename);
                doneDifference.RemovedFiles.Add(item);
            }
        }
    }
}