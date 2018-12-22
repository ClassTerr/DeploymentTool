using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models.Enums
{
    public enum DeploySessionState
    {
        Opened = 0,
        InProcess = 2,
        Done = 3,
        Error = 4,
        CriticalError = 4,
        NotUsed = 6
    }
}
