using DeploymentTool.Core.Filesystem;
using DeploymentTool.Core.Models;
using DeploymentTool.Core.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DeploymentTool.API.Services
{
    public static class BackupService
    {
        private static readonly string BackupFolder = Path.GetFullPath("Backups");

        public static BackupResult CreateBackup(ClientProfile profile, FilesystemDifference difference)
        {
            if (difference.CreatedFiles.Length +
                difference.ModifiedFiles.Length +
                difference.RemovedFiles.Length == 0)
            {
                return null;
            }

            var backupFolderPath = InitBackupFolder(profile);
            var profilePath = FilesystemUtils.NormalizePath(profile.RootFolder);
            var logFileName = Path.Combine(backupFolderPath, "Log.txt");
            var revertFileName = Path.Combine(backupFolderPath, "rollback.dat");

            BackupResult backupResult = new BackupResult()
            {
                Errors = new List<string>(),
                BackupFolder = backupFolderPath
            };

            try
            {
                using (var logWriter = new StreamWriter(logFileName))
                {
                    logWriter.Write(logFileName, "Backup Initialized: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\n");
                    logWriter.WriteLine("CreatedFiles: " + difference.CreatedFiles.Length);
                    logWriter.WriteLine("ModifiedFiles: " + difference.ModifiedFiles.Length);
                    logWriter.WriteLine("RemovedFiles: " + difference.RemovedFiles.Length);

                    GenerateRollback(revertFileName, difference);

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
                                backupResult.Errors.Add(error);
                                logWriter.Write(error);
                            }
                        }
                        else
                        {
                            var error = $"Can not find file {filePath}";
                            backupResult.Errors.Add(error);
                            logWriter.Write(error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                backupResult.Errors.Add($"Critical exception has occured: {ex}");
            }
            return backupResult;
        }

        private static void GenerateRollback(string revertFileName, FilesystemDifference difference)
        {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>Name of created folder</returns>
        public static string InitBackupFolder(ClientProfile profile)
        {
            //TODO ADD PROFILE BACKUP FOLDER
            var now = DateTime.Now;

            InitFolder(BackupFolder);

            string todayFolder = Path.Combine(BackupFolder, now.ToString("yyyy-MM-dd"));

            InitFolder(todayFolder);

            string nowFolder = Path.Combine(todayFolder, now.ToString("HH-mm-ss"));

            InitFolder(nowFolder);

            return nowFolder;
        }

        private static void InitFolder(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
        }
    }
}