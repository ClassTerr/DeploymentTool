using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Helpers
{
    public static class ObjectExtender
    {
        public static string ToJSON(this object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
