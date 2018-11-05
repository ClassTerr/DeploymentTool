using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Filesystem
{
    public static class FilesystemUtils
    {
        public static List<string> GetDirectoryFiles(string directoryPath)
        {
            return GetDirectoryFiles(directoryPath, null, new List<string>());
        }

        public static List<string> GetDirectoryFiles(string directoryPath, IEnumerable<string> excludedPaths)
        {
            return GetDirectoryFiles(directoryPath, excludedPaths, new List<string>());
        }

        public static List<string> GetDirectoryFiles(string directoryPath, IEnumerable<string> excludedPaths, List<string> existingResult)
        {
            if (!Directory.Exists(directoryPath))
                return existingResult;

            if (excludedPaths == null)
                excludedPaths = new string[0];

            //clear all unused paths for optimization purposes
            excludedPaths = excludedPaths.Where(path => path.StartsWith(directoryPath));

            string[] files = Directory.GetFiles(directoryPath);

            //add to result all non-excluded files
            existingResult.AddRange(files.Where(file => !excludedPaths.Where(file.StartsWith).Any()));

            string[] subDirectories = Directory.GetDirectories(directoryPath);

            foreach (var subDirectory in subDirectories)
            {
                GetDirectoryFiles(subDirectory, excludedPaths, existingResult);
            }

            return existingResult;
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }
    }
}
