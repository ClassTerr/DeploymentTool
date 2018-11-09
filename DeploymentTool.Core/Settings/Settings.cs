using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeploymentTool.Core.Settings
{
    [Serializable]
    public class SettingsBase
    {
        public List<ClientProfile> Profiles { get; set; } = new List<ClientProfile>();

        public ClientProfile GetProfile(string profileId)
        {
            return Profiles?.FirstOrDefault(profile => profile.ID == profileId);
        }

        public bool UpdateProfile(ClientProfile profile)
        {
            return UpdateProfile(profile, profile.ID);
        }

        public bool UpdateProfile(ClientProfile profile, string profileID)
        {
            var index = Profiles.FindIndex(x => x.ID == profileID);
            if (index != -1)
            {
                Profiles[index] = profile;
            }

            return index != -1;
        }

        public void RemoveProfile(ClientProfile profile)
        {
            RemoveProfile(profile.ID);
        }

        public void RemoveProfile(string Id)
        {
            var index = Profiles.FindIndex(x => x.ID == Id);
            Profiles.RemoveAt(index);
        }
    }

    [Serializable]
    public class ClientSettings
    {

    }

    public class ClientProfile
    {
        [XmlAttribute]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [XmlAttribute]
        public string Name { get; set; }
        public string RootFolder { get; set; }
        public List<string> ExcludedPaths { get; set; }
        public string APICommand { get; set; }
        public string UpadateAPISSH { get; set; }

        public override string ToString()
        {
            return Name ?? "Profile";
        }
    }


    public class ServerSettings
    {
        public List<ServerProfile> Profiles { get; set; } = new List<ServerProfile>();

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
    }

    public class ServerProfile : ClientProfile
    {
        public List<BackupResult> Backups { get; set; }
        public string BackupFolder { get; set; }
    }
}
