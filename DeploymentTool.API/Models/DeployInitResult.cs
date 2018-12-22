using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeploymentTool.API.Models
{
    public class DeployInitResult
    {
        public FilesystemDifference FilesystemDifference { get; set; }
        public string DeploySessionID { get; set; }
        public DateTime Expires { get; set; }
    }
}