using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DeploymentTool
{
    [Serializable]
    public class Settings
    {
        public List<Profile> Profiles { get; set; } = new List<Profile>();

        public Profile GetProfile(string profileName)
        {
            return Profiles?.FirstOrDefault(profile => profile.Name == profileName);
        }
    }


    public class Profile
    {
        [XmlAttribute]
        public string Name { get; set; }
        public List<string> IncludedPaths { get; set; }
        public List<string> ExcludedPaths { get; set; }
        public string UpadateAPIURL { get; set; }
        public string UpadateAPISSH { get; set; }

        public override string ToString()
        {
            return Name ?? "Profile";
        }
    }
}
