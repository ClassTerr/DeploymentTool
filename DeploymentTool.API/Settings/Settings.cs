using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace DeploymentTool.Settings
{
    [Serializable]
    public class Settings
    {
        public Settings()
        {
            BackupsFolder = ConfigurationManager.AppSettings.Get("backupsFolder");
            DeploySessionFolder = ConfigurationManager.AppSettings.Get("deploySessionFolder");
        }

        public List<ServerProfile> Profiles { get; set; } = new List<ServerProfile>();
        public List<DeploySession> DeploySessions { get; set; } = new List<DeploySession>();
        public List<Token> Tokens { get; set; } = new List<Token>();

        public string BackupsFolder { get; set; } = Path.GetFullPath("Backups");
        public string DeploySessionFolder { get; set; } = Path.GetFullPath("Deploys");
        public string AccessToken { get; set; }

        public ServerProfile GetProfile(string profileId)
        {
            return Profiles?.FirstOrDefault(profile => profile.ID == profileId);
        }

        public bool UpdateProfile(ServerProfile profile)
        {
            return UpdateProfile(profile, profile.ID);
        }

        public bool UpdateProfile(ServerProfile profile, string profileID)
        {
            var index = Profiles.FindIndex(x => x.ID == profileID);
            if (index != -1)
            {
                Profiles[index] = profile;
            }

            return index != -1;
        }

        public void RemoveProfile(ServerProfile profile)
        {
            RemoveProfile(profile.ID);
        }

        public void RemoveProfile(string Id)
        {
            var index = Profiles.FindIndex(x => x.ID == Id);
            Profiles.RemoveAt(index);
        }

        public bool IsDeployingNow
        {
            get
            {
                return DeploySessions.Any(x => !x.IsDeployed);
            }
        }

        public void StopDeploying()
        {
            foreach (var session in DeploySessions)
            {
                session.IsDeployed = true;
            }
        }
    }
}
