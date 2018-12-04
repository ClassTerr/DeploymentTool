using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class DeploySession
    {
        public string Id { get; set; }
        public string ProfileId { get; set; }
        public string ProfileName { get; set; }
        public bool IsDeployed { get; set; } = false;
        public FilesystemDifference FilesystemDifference { get; set; }
        public DateTime Expires => Created.Add(AliveTime);
        public DateTime Created { get; set; } = DateTime.Now;
        public bool IsExpired => Expires < DateTime.Now || IsDeployed;
        
        public string GetDirectoryName()
        {
            var name = Created.ToString() + " " + ProfileName + " " + Id;

            //replace all not allowed chars to '-'
            var notAllowedChars = new HashSet<char>(Path.GetInvalidFileNameChars());
            name = new string(name.Select(c => notAllowedChars.Contains(c) ? '-' : c).ToArray());

            return name;
        }

        /// <summary>
        /// By default is two hour
        /// </summary>
        public static readonly TimeSpan AliveTime = TimeSpan.FromDays(365);//for testing
        //public static readonly TimeSpan AliveTime = TimeSpan.FromHours(2); //TODO revert
    }
}
