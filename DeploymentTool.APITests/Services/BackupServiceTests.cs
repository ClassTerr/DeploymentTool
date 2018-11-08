using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentTool.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeploymentTool.Core.Settings;
using System.IO;
using DeploymentTool.Core.Models;

namespace DeploymentTool.API.Services.Tests
{
    [TestClass()]
    public class BackupServiceTests
    {
        [TestMethod()]
        public void CreateBackupTest()
        {
            
            string folder = Path.Combine(Path.GetTempPath(), "DeploymentToolTesting");
            string filename = Path.Combine(folder, "test.txt");

            if (!Directory.Exists(folder))  
            {
                Directory.CreateDirectory(folder);
            }

            File.Delete(filename);

            Profile profile = new Profile()
            {
                ID = "Test Profile",
                RootFolder = folder
            };
            
            using (File.Create(filename)) { }
            var state1 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            File.Delete(filename);
            var state2 = FilesystemStateModel.GetFullProfileFilesystemState(profile);

            var diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);

            BackupService.CreateBackup(profile, diff);
        }
    }
}