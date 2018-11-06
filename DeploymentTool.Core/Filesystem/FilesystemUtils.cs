using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Filesystem
{
    public static class FilesystemUtils
    {
        public static List<string> GetDirectoryFiles(string directoryPath)
        {
            return GetDirectoryFiles(directoryPath, null, new List<string>());
        }

        public static List<string> GetDirectoryFiles(string directoryPath, IEnumerable<string> excludedRules)
        {
            return GetDirectoryFiles(directoryPath, excludedRules, new List<string>());
        }

        public static List<string> GetDirectoryFiles(string directoryPath, IEnumerable<string> excludedRules, List<string> existingResult)
        {
            if (!Directory.Exists(directoryPath))
                return existingResult;

            if (excludedRules == null)
                excludedRules = new string[0];

            //clear all unused paths for optimization purposes
            excludedRules = excludedRules.Where(path => path.StartsWith(directoryPath));

            string[] files = Directory.GetFiles(directoryPath);

            //add to result all non-excluded files
            existingResult.AddRange(files.Where(file => !excludedRules.Where(file.StartsWith).Any()));

            string[] subDirectories = Directory.GetDirectories(directoryPath);

            foreach (var subDirectory in subDirectories)
            {
                GetDirectoryFiles(subDirectory, excludedRules, existingResult);
            }

            return existingResult;
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

        public static void GetsAllFilesAndDirectories(string rootPath, bool recursively = true)
        {
            string searchPattern = "*";


            SearchOption option = recursively 
                ? SearchOption.AllDirectories 
                : SearchOption.TopDirectoryOnly;

            DirectoryInfo di = new DirectoryInfo(rootPath);
            DirectoryInfo[] directories =
                di.GetDirectories(searchPattern, option);

            FileInfo[] files =
                di.GetFiles(searchPattern, option);

            var allData = files.Cast<FileSystemInfo>()
                .Union(directories.Cast<FileSystemInfo>())
                .OrderBy(x => x.FullName).ToList();
        }

        public static bool TestExcluded(string path, IEnumerable<string> excludedRules)
        {
            return excludedRules.Any(rule =>
                Regex.IsMatch(path, rule)
            );
        }
    }
}
