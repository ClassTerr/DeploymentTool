using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_TEST_CONSOLE
{
    public class Globals
    {
        public readonly static string API_KEY, SITE_URL;
        static Globals()
        {
            API_KEY = ConfigurationManager.AppSettings.Get("API_KEY");
            SITE_URL = ConfigurationManager.AppSettings.Get("SITE_URL");
        }
    }
}
