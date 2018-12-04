using DeploymentTool.Core.Models;
using DeploymentTool.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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

            ServerProfile profile = new ServerProfile()
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
            var state1 = FilesystemStateModel.GetProfileFilesystemState(profile);
            File.Delete(filename);
            var state2 = FilesystemStateModel.GetProfileFilesystemState(profile);

            using (File.Create(filename)) { }
            var diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);

            BackupService.CreateBackup(profile, diff);
            Assert.IsTrue(true);
        }
    }
}