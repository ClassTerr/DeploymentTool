using DeploymentTool.Core.Filesystem;
using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
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

            var backupFolderPath = InitBackupFolder(session);
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

        public static void GenerateRollback(string backupFolder, FilesystemDifference difference)
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


        public static string InitBackupFolder(DeploySession session)
        {
            string targetFolder = Path.Combine(SettingsManager.Instance.BackupsFolder, session.GetDirectoryName());
            Directory.CreateDirectory(targetFolder);

            return targetFolder;
        }
    }
}