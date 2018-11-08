using Microsoft.VisualStudio.TestTools.UnitTesting;
using DeploymentTool.Core.Filesystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DeploymentTool.Core.Models;

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


            var state1 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);
            var state2 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);
            var diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 0, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);
            using (File.Create(filename)) { }
            state2 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 1, 0, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);
            File.WriteAllText(filename, "test");
            state2 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);

            diff = FilesystemStateModel.GetFilesystemStateDiff(state1, state2);
            GetFilesystemStateDiffTestAsserts(diff, 0, 1, 0);

            /////////////////////////////////////////////

            state1 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);
            File.Delete(filename);
            state2 = FilesystemStateModel.GetFullFolderFilesystemState(folder, null);

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