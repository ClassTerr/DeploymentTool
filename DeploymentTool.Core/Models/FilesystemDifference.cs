using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class FilesystemDifference
    {
        public List<FileDataModel> CreatedFiles { get; set; } = new List<FileDataModel>();
                                 
        public List<FileDataModel> RemovedFiles { get; set; } = new List<FileDataModel>();

        public List<FileDataModel> ModifiedFiles { get; set; } = new List<FileDataModel>();
    }
}
