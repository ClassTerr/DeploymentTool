using DeploymentTool.Settings;
using System;
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

            Application.Run(new ProfilesManagerWindow());
        }
    }
}
