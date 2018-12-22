using DeploymentTool.API.Settings;
using DeploymentTool.Core.Models;
using DeploymentTool.Core.Models.Enums;
using System;
using System.IO;
using System.Linq;

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
        public static void Deploy(string sessionId)
        {
            Deploy(GetDeploySession(sessionId));
        }

        public static void Deploy(DeploySession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            var sessionDirectoryName = session.GetDirectoryName();

            ServerProfile profile = ProfileService.GetProfileById(session.ProfileId);
            string targetFolder = profile?.RootFolder;
            string sourceFolder = Path.Combine(SettingsManager.Instance.DeploySessionFolder, sessionDirectoryName);
            string backupFolder = Path.Combine(SettingsManager.Instance.BackupsFolder, sessionDirectoryName);

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

            FilesystemDifference difference = session.FilesystemDifference;

            var backupFolderName = BackupService.InitBackupDirectory(session);


            //if some error will be occuerd we need to rollback
            //so we write rollback script only for already processed files
            //this variable will be changed in BackupService.MoveFiles method
            FilesystemDifference realDifference = new FilesystemDifference();

            try
            {
                try
                {
                    session.State = DeploySessionState.InProcess;
                    BackupService.MoveFiles(sourceFolder, targetFolder, backupFolder, difference, realDifference);
                    session.State = DeploySessionState.Done;
                }
                catch (Exception e)
                {
                    session.State = DeploySessionState.Error;
                    throw e;
                }
                finally
                {
                    BackupService.GenerateRollbackScript(backupFolder, realDifference);
                }
            }
            catch (Exception e)
            {
                // if some errors occured while deploying - revert all changes
                try
                {
                    BackupService.Rollback(session);
                }
                catch (Exception critException)
                {
                    session.State = DeploySessionState.CriticalError;
                    throw new Exception("Critical error was occured! Requires manual resolving." +
                        " For more info see log for session id=" + session.Id, critException);
                }
            }
        }
    }
}