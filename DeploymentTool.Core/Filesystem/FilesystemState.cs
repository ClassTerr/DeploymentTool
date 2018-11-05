using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Filesystem
{
    public class FilesystemState
    {
        public FilesystemState()
        {

        }

        public static void /*FilesystemState*/ GetFilesystemState(IEnumerable<string> includedPaths, IEnumerable<string> excludedPaths)
        {
            includedPaths = includedPaths.Select(Path.GetFullPath);
            excludedPaths = excludedPaths.Select(Path.GetFullPath);

            List<string> allFilenames = new List<string>();

            foreach (var includedPath in includedPaths)
            {
                if (File.Exists(includedPath))
                {
                    if (!excludedPaths.Contains(includedPath))
                    {
                        allFilenames.Add(includedPath);
                    }
                }
                else
                {
                    FilesystemUtils.GetDirectoryFiles(includedPath, excludedPaths, allFilenames);
                }
            }

        }

    }
}
