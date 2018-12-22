using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class ProfileModel
    {
        public string ID { get; set; } = Guid.NewGuid().ToString();
        
        public string Name { get; set; }

        public List<string> ExcludedPaths { get; set; } = new List<string>();
    }
}
