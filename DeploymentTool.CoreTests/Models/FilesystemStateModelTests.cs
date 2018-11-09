using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentTool.Core.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DeploymentTool.Core.Models;
using DeploymentTool.Core.Settings;

namespace DeploymentTool.Core.Filesystem.Tests
{
    [TestClass()]
    public class FilesystemStateModelTests
    {
        [TestMethod()]
        public void GetFilesystemStateDiffTest()
        {
            string folder = Path.Combine(Path.GetTempPath(), "DeploymentToolTesting");
            string filename = Path.Combine(folder, "test.txt");

            if (!Directory.Exists(folder))  
            {
                Directory.CreateDirectory(folder);
            }

            File.Delete(filename);

            ClientProfile profile = new ClientProfile()
            {
                ID = "Test Profile",
                RootFolder = folder
            };

            var state1 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            var state2 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 0, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            using (File.Create(filename)) { }
            state2 = FilesystemStateModel.GetFullProfileFilesystemState(profile);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 1, 0, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            File.WriteAllText(filename, "test");
            state2 = FilesystemStateModel.GetFullProfileFilesystemState(profile);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 1, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetFullProfileFilesystemState(profile);
            File.Delete(filename);
            state2 = FilesystemStateModel.GetFullProfileFilesystemState(profile);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 0, 1);

        }
        public void GetFilesystemStateDiffTestAsserts(FilesystemDifference diff, int created, int modified, int removed)
        {
            Assert.AreEqual(diff.CreatedFiles.Length, created);
            Assert.AreEqual(diff.ModifiedFiles.Length, modified);
            Assert.AreEqual(diff.RemovedFiles.Length, removed);
        }
    }
}