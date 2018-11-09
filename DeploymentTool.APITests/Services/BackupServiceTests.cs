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

            if (!Directory.Exists(folder))  
            {
                Directory.CreateDirectory(folder);
            }

            ClientProfile profile = new ClientProfile()
            {
                ID = "Test Profile",
                RootFolder = folder
            };

            folder = Path.Combine(folder, "test");
            if (!Directory.Exists(folder))  
            {
                Directory.CreateDirectory(folder);
            }

            string filename = Path.Combine(folder, "test.txt");

            File.Delete(filename);

            
            using (File.Create(filename)) { }
            var state1 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            File.Delete(filename);
            var state2 = FilesystemStateModel.GetFullProfileFilesystemState(profile);

            using (File.Create(filename)) { }
            var diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);

            var result = BackupService.CreateBackup(profile, diff);
            Assert.AreEqual(result.Errors.Count, 0);
        }
    }
}