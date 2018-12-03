using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DeploymentTool.Core.Models
{
    public class ProfileBase
    {
        [XmlAttribute]
        public string ID { get; set; } = Guid.NewGuid().ToString();

        [XmlAttribute]
        public string Name { get; set; }

        public string RootFolder { get; set; }

        public List<string> ExcludedPaths { get; set; } = new List<string>();
    }
}
