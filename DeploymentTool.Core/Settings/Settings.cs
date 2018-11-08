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
    public class Settings
    {
        public List<Profile> Profiles { get; set; } = new List<Profile>();

        public Profile GetProfile(string profileId)
        {
            return Profiles?.FirstOrDefault(profile => profile.ID == profileId);
        }

        public bool UpdateProfile(Profile profile)
        {
            return UpdateProfile(profile, profile.ID);
        }

        public bool UpdateProfile(Profile profile, string profileID)
        {
            var index = Profiles.FindIndex(x => x.ID == profileID);
            if (index != -1)
            {
                Profiles[index] = profile;
            }

            return index != -1;
        }

        public void RemoveProfile(Profile profile)
        {
            RemoveProfile(profile.ID);
        }

        public void RemoveProfile(string Id)
        {
            var index = Profiles.FindIndex(x => x.ID == Id);
            Profiles.RemoveAt(index);
        }
    }


    public class Profile
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
}
