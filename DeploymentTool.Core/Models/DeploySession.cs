using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class DeploySession
    {
        public string Id { get; set; }
        public string ProfileId { get; set; }
        public List<string> Files { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsExpired => EndDate < DateTime.Now;
    }
}
