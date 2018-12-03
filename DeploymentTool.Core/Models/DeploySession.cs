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
        public DateTime Expires => Created.Add(AliveTime);
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsExpired => Expires < DateTime.Now;

        /// <summary>
        /// By default is two hour
        /// </summary>
        public static readonly TimeSpan AliveTime = TimeSpan.FromDays(365);//for testing
        //public static readonly TimeSpan AliveTime = TimeSpan.FromHours(2); //TODO revert
    }
}
