using DeploymentTool.Core.Filesystem;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace DeploymentTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string settingsPath = ConfigurationManager.AppSettings.Get("settingsFilePath");
            SettingsManager.ConfigFilePath = settingsPath;

            Application.Run(new ProfilesManagerWindow());
        }
    }
}
