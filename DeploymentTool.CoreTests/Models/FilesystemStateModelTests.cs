using DeploymentTool.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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

            ProfileBase profile = new ProfileBase()
            {
                ID = "Test Profile",
                RootFolder = folder
            };

            var state1 = FilesystemStateModel.GetProfileFilesystemState(profile);
            var state2 = FilesystemStateModel.GetProfileFilesystemState(profile);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 0, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetProfileFilesystemState(profile);
            using (File.Create(filename)) { }
            state2 = FilesystemStateModel.GetProfileFilesystemState(profile);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 1, 0, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetProfileFilesystemState(profile);
            File.WriteAllText(filename, "test");
            state2 = FilesystemStateModel.GetProfileFilesystemState(profile);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 1, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetProfileFilesystemState(profile);
            File.Delete(filename);
            state2 = FilesystemStateModel.GetProfileFilesystemState(profile);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 0, 1);

        }
        public void GetFilesystemStateDiffTestAsserts(FilesystemDifference diff, int created, int modified, int removed)
        {
            Assert.AreEqual(diff.CreatedFiles.Count, created);
            Assert.AreEqual(diff.ModifiedFiles.Count, modified);
            Assert.AreEqual(diff.RemovedFiles.Count, removed);
        }
    }
}