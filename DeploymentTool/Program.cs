using DeploymentTool.Core.Settings;
using System;
using System.Configuration;
using System.IO;
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
            SettingsManager<Settings>.ConfigFilePath = settingsPath;

            Application.Run(new ProfilesManagerWindow());
        }
    }
}
