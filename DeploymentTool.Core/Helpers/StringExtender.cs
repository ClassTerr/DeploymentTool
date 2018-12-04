using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Helpers
{
    public static class StringExtender
    {
        public static object ToObject(this string str)
        {
            return str.ToObject<object>();
        }

        public static T ToObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }

        public static bool IsPathEqualTo(this string path1, string path2)
        {
            path1 = Path.GetFullPath(path1);
            path2 = Path.GetFullPath(path2);

            return string.Equals(path1, path2, StringComparison.OrdinalIgnoreCase);
        }

    }
}
