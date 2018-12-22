using DeploymentTool.Core.Models;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DeploymentTool.API.Settings
{
    [XmlRoot(ElementName = "Profile")]
    public class ServerProfile : ProfileBase
    {
        public ServerProfile() { }
        public ServerProfile(ProfileBase @base)
        {
            ID = @base.ID;
            Name = @base.Name;
            ExcludedPaths = @base.ExcludedPaths;
        }
        public string URL { get; set; }
        public string UpadateAPISSH { get; set; }
        public List<OperationResult> Backups { get; set; }

        public override string ToString()
        {
            return Name ?? "Profile";
        }
    }
}
