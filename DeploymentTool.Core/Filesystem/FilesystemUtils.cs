using DeploymentTool.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Filesystem
{
    public static class FilesystemUtils
    {
        public static FileDataModel[] GetAllAllowedFilesDataModel(string rootPath, IEnumerable<string> exclusionRules)
        {
            return GetAllAllowedFilesInfo(rootPath, exclusionRules)
                .Select(FileDataModel.Calculate).ToArray();
        }

        public static FileInfo[] GetAllAllowedFilesInfo(string rootPath, IEnumerable<string> exclusionRules)
        {
            var allPaths = GetAllFilesInfo(rootPath, true);

            if (!allPaths.Any())
                return new FileInfo[0];

            exclusionRules = ExclusionRules.ConvertToRegexp(exclusionRules);

            var result = GetAllowedPaths(allPaths, exclusionRules);
            return result;
        }

        public static List<string> GetDirectoryFiles(string directoryPath)
        {
            return GetAllowedDirectoryFiles(directoryPath, null, null);
        }

        public static List<string> GetAllowedDirectoryFiles(string directoryPath, IEnumerable<string> exclusionRules)
        {
            return GetAllowedDirectoryFiles(directoryPath, exclusionRules, null);
        }

        public static List<string> GetAllowedDirectoryFiles(string directoryPath,
                                                     IEnumerable<string> exclusionRules,
                                                     List<string> existingResult)
        {
            if (!Directory.Exists(directoryPath))
                return existingResult;

            if (exclusionRules == null)
                exclusionRules = new string[0];

            string[] files = Directory.GetFiles(directoryPath);

            //add to result all non-excluded files
            existingResult.AddRange(files.Where(file => !exclusionRules.Where(file.StartsWith).Any()));

            string[] subDirectories = Directory.GetDirectories(directoryPath);

            foreach (var subDirectory in subDirectories)
            {
                GetAllowedDirectoryFiles(subDirectory, exclusionRules, existingResult);
            }

            return existingResult;
        }

        public static string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToUpperInvariant();
        }

        public static FileInfo[] GetAllFilesInfo(string rootPath, bool recursively = true)
        {
            string searchPattern = "*";


            SearchOption option = recursively
                ? SearchOption.AllDirectories
                : SearchOption.TopDirectoryOnly;

            DirectoryInfo di = new DirectoryInfo(rootPath);

            FileInfo[] files = di.GetFiles(searchPattern, option);

            return files;
        }

        public static FileSystemInfo[] GetAllFilesAndDirectories(string rootPath, bool recursively = true)
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
                .OrderBy(x => x.FullName).ToArray();

            return allData;
        }

        public static FileInfo[] GetAllowedPaths(IEnumerable<FileInfo> paths, IEnumerable<string> exclusionRules)
        {
            return paths.Where(path => !TestExcluded(path.FullName, exclusionRules)).ToArray();
        }

        public static bool TestExcluded(string path, IEnumerable<string> exclusionRules)
        {
            if (exclusionRules == null)
            {
                exclusionRules = new string[0];
            }

            return exclusionRules.Any(rule =>
                Regex.IsMatch(path, rule)
            );
        }

        public static string CalculateMD5(string filename)
        {
            using (var md5 = MD5.Create())
            {
                try
                {
                    using (var stream = File.OpenRead(filename))
                    {
                        var hash = md5.ComputeHash(stream);
                        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                    }
                }
                catch
                {
                    return null;
                }
            }
        }

    }
}
