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

        public static List<string> CreateBackup(Profile profile, FilesystemDifference difference)
        {
            var errors = new List<string>();
            if (difference.CreatedFiles.Length +
                difference.ModifiedFiles.Length +
                difference.RemovedFiles.Length == 0)
            {
                return errors;
            }

            try
            {
                var backupFolderPath = InitBackupFolder(profile);

                foreach (var item in difference.RemovedFiles.Union(difference.ModifiedFiles))
                {
                    var filePath = Path.Combine(profile.RootFolder, item.Filename);
                    if (File.Exists(filePath))
                    {
                        var fileDirectory = Path.GetDirectoryName(filePath);
                        var filename = Path.GetFileName(filePath);
                        var backupFileDirectory = Path.Combine(backupFolderPath, fileDirectory);
                        var targetFilePath = Path.Combine(backupFileDirectory, filename);

                        FilesystemUtils.CreateDirectory(backupFileDirectory);
                        try
                        {
                            File.Copy(filePath, targetFilePath);
                        }
                        catch (Exception e)
                        {
                            errors.Add($"Errors occured when attempt to backup file {filename} form {filePath} to {targetFilePath}\r\n exception: {e}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add($"Critical exception has occured: {ex}");
            }
            return errors;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile"></param>
        /// <returns>Name of created folder</returns>
        public static string InitBackupFolder(Profile profile)
        {
            var now = DateTime.Now;

            InitFolder(BackupFolder);

            string todayFolder = Path.Combine(BackupFolder, now.ToString("yyyy-MM-dd"));

            InitFolder(todayFolder);

            string nowFolder = Path.Combine(todayFolder, now.ToString("HH-mm-ss"));

            InitFolder(nowFolder);

            var logFileName = Path.Combine(todayFolder, "Log.txt");
            File.WriteAllText(logFileName, "Backup Initialized: " + now.ToString() + "\n");

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