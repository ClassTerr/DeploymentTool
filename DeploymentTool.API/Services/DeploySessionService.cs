using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DeploymentTool.API.Services
{
    public static class DeploySessionService
    {
        public static DeploySession GetDeploySession(string id)
        {
            return SettingsManager.Instance.DeploySessions.FirstOrDefault(x => x.Id == id);
        }

        public static DeploySession CreateDeploySession(string profileId)
        {
            var profile = ProfileService.GetProfileById(profileId);
            if (profile == null)
            {
                throw new ArgumentException("Profile not found!");
            }

            DeploySession session = new DeploySession()
            {
                ProfileId = profileId,
                ProfileName = profile.Name
            };

            SettingsManager.Instance.DeploySessions.Add(session);
            SettingsManager.SaveConfig();

            return session;
        }
        public static void DoDeploy(string sessionId)
        {
            DoDeploy(GetDeploySession(sessionId));
        }

        public static void DoDeploy(DeploySession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            var sessionDirectoryName = session.GetDirectoryName();

            ServerProfile profile = ProfileService.GetProfileById(session.ProfileId);
            string targetFolder = profile.RootFolder;
            string sourceFolder = Path.Combine(SettingsManager.Instance.DeploySessionFolder, sessionDirectoryName);
            string backupFolder = Path.Combine(SettingsManager.Instance.BackupsFolder, sessionDirectoryName);

            if (profile == null)
            {
                throw new Exception("Profile not valid");
            }

            if (!Directory.Exists(sourceFolder))
            {
                throw new DirectoryNotFoundException("There is no directory with deploying files");
            }

            if (!Directory.Exists(targetFolder))
            {
                throw new DirectoryNotFoundException("There is no destination directory");
            }
            
            FilesystemDifference difference = session.FilesystemDifference;

            var backupFolderName = BackupService.InitBackupFolder(session);


            //if errors occuerd we need to write rollback script only for processed files
            FilesystemDifference realDifference = new FilesystemDifference();

            try
            {
                // copy all files from source folder 
                MoveFiles(sourceFolder, targetFolder, backupFolder, difference, realDifference);
            }
            finally
            {
                BackupService.GenerateRollback(backupFolder, realDifference);
            }
        }

        private static void MoveFiles(string sourceFolder, string targetFolder, string backupFolder, FilesystemDifference difference, FilesystemDifference doneDifference)
        {
            var filesToDeploy = difference.CreatedFiles.Union(difference.ModifiedFiles);

            foreach (var item in filesToDeploy)
            {
                var sourceFilename = Path.Combine(sourceFolder, item.Filename);
                var targetFilename = Path.Combine(targetFolder, item.Filename);
                var backupFilename = Path.Combine(backupFolder, item.Filename);

                if (File.Exists(targetFilename))
                {
                    File.Replace(sourceFilename, targetFilename, backupFilename);
                    doneDifference.ModifiedFiles.Add(item);
                }
                else
                {
                    File.Move(sourceFilename, targetFilename);
                    doneDifference.CreatedFiles.Add(item);
                }
            }

            foreach (var item in difference.RemovedFiles)
            {
                var targetFilename = Path.Combine(targetFolder, item.Filename);
                File.Delete(targetFilename);
                doneDifference.RemovedFiles.Add(item);
            }
        }
    }
}