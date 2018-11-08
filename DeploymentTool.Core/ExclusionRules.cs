using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeploymentTool.Core
{
    public static class ExclusionRules
    {
        public static string[] ConvertToRegexp(IEnumerable<string> rules)
        {
            return rules?.Select(rule => ConvertToRegexp(rule)).ToArray();
        }

        public static string ConvertToRegexp(string rule)
        {
            if (string.IsNullOrEmpty(rule))
                return rule;

            rule = Regex.Escape(rule);

            // keep "*" as regex rule
            rule = rule.Replace(@"\*", "*");
            
            rule = "^" + rule + "$";

            return rule;
        }
    }
}
