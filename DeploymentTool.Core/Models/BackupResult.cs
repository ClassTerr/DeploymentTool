using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class OperationResult
    {
        public List<string> Errors { get; } = new List<string>();

        public bool IsSuccess
        {
            get
            {
                return !Errors.Any();
            }
        }

        public override string ToString()
        {
            return String.Join(Environment.NewLine, Errors);
        }
    }
}
