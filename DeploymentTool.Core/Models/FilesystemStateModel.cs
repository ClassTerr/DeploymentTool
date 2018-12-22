using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DeploymentTool.Core.Filesystem;
using DeploymentTool.Core.Models;
using DeploymentTool.Core.Helpers;

namespace DeploymentTool.Core.Models
{
    public class FilesystemStateModel
    {
        public FilesystemStateModel()
        {

        }

        public FileDataModel[] FileStates { get; set; }
        public string ProfileID { get; set; }
        public DateTime CreatedUTC { get; set; } = DateTime.UtcNow;
        
        public static FilesystemStateModel GetProfileFilesystemState(ProfileBase profile, string rootPath = null)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            if (rootPath == null)
            {
                rootPath = FilesystemUtils.NormalizePath(profile.RootFolder);
            }

            var result = new FilesystemStateModel()
            {
                ProfileID = profile.ID,
                CreatedUTC = DateTime.UtcNow
            };

            rootPath = FilesystemUtils.NormalizePath(rootPath);
            
            FileDataModel[] fileDataModels = FilesystemUtils.GetAllAllowedFilesDataModel(rootPath, profile.ExcludedPaths);
            foreach (FileDataModel model in fileDataModels)
            {
                var filename = FilesystemUtils.NormalizePath(model.Filename);
                if (filename.StartsWith(rootPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    model.Filename = filename.Substring(rootPath.Length)
                        .Trim(new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });
                }
            }

            result.FileStates = fileDataModels;
            return result;
        }

        public static FilesystemDifference GetFilesystemStateDiff(FilesystemStateModel currentState,
                                                                  FilesystemStateModel remoteState)
        {
            return GetFilesystemStateDiff(currentState.FileStates, remoteState.FileStates);
        }

        public static FilesystemDifference GetFilesystemStateDiff(IEnumerable<FileDataModel> currentFileStates,
                                                                  IEnumerable<FileDataModel> remoteFileStates)
        {
            var notModifiedFiles = currentFileStates.Intersect(remoteFileStates);

            var currentChangedFiles = currentFileStates.Except(notModifiedFiles);
            var remoteChangedFiles = remoteFileStates.Except(notModifiedFiles);

            remoteFileStates = remoteFileStates.Except(notModifiedFiles);

            var currentNameDict = currentChangedFiles.ToDictionary(x => x.Filename);                       // O(log n)
            var remoteNameDict = remoteChangedFiles.ToDictionary(x => x.Filename);                         // O(log n)

            var createdFiles = remoteChangedFiles.Where(x => !currentNameDict.ContainsKey(x.Filename));    // O(n * log m)
            var removedFiles = currentFileStates.Where(x => !remoteNameDict.ContainsKey(x.Filename));      // O(n * log m)

            var modifiedFiles = currentChangedFiles.Where(x =>
            {
                if (remoteNameDict.TryGetValue(x.Filename, out FileDataModel model))
                {
                    return x.MD5 != model.MD5;
                }

                return false;
            });

            FilesystemDifference diff = new FilesystemDifference
            {
                CreatedFiles = createdFiles.ToList(),
                RemovedFiles = removedFiles.ToList(),
                ModifiedFiles = modifiedFiles.ToList()
            };

            return diff;
        }
    }
}
