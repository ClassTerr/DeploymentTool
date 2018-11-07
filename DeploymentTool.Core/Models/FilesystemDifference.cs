using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class FilesystemDifference
    {
        public FileDataModel[] CreatedFiles { get; set; }

        public FileDataModel[] RemovedFiles { get; set; }

        public FileDataModel[] ModifiedFiles { get; set; }
    }
}
