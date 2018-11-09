using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class BackupResult
    {
        public List<string> Errors { get; set; }
        public string BackupFolder { get; set; }

        public bool IsSuccess
        {
            get
            {
                return !Errors.Any();
            }
        }
    }
}
