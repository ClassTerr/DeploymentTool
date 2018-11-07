using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DeploymentTool.Core.Filesystem;
using DeploymentTool.Core.Models;
using DeploymentTool.Core.Helpers;

namespace DeploymentTool.Core.Filesystem
{
    public class FilesystemStateModel
    {
        public FilesystemStateModel()
        {

        }

        public FileDataModel[] FileStates { get; set; }

        public DateTime SnapshotDateTime { get; set; }

        public static FilesystemStateModel GetFullFolderFilesystemState(string rootPath, IEnumerable<string> exclusionRules)
        {
            rootPath = FilesystemUtils.NormalizePath(rootPath);

            var result = new FilesystemStateModel();

            FileDataModel[] fileDataModels = FilesystemUtils.GetAllAllowedFilesDataModel(rootPath, exclusionRules);
            foreach (FileDataModel model in fileDataModels)
            {
                var filename = FilesystemUtils.NormalizePath(model.Filename);
                if (filename.StartsWith(rootPath))
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

            var currentChangedFileStates = currentFileStates.Except(notModifiedFiles);
            var remoteChangedFileStates = remoteFileStates.Except(notModifiedFiles);

            remoteFileStates = remoteFileStates.Except(notModifiedFiles);

            var currentNameDict = currentChangedFileStates.ToDictionary(x => x.Filename);                       // O(log n)
            var remoteNameDict = remoteChangedFileStates.ToDictionary(x => x.Filename);                         // O(log n)
                                                                                                                   
            var createdFiles = remoteChangedFileStates.Where(x => !currentNameDict.ContainsKey(x.Filename));    // O(n * log m)
            var removedFiles = currentFileStates.Where(x => !remoteNameDict.ContainsKey(x.Filename));           // O(n * log m)

            var modifiedFiles = currentChangedFileStates.Where(x =>
            {
                if (remoteNameDict.TryGetValue(x.Filename, out FileDataModel model))
                {
                    return x.MD5 != model.MD5;
                }

                return false;
            });

            FilesystemDifference diff = new FilesystemDifference
            {
                CreatedFiles = createdFiles.ToArray(),
                RemovedFiles = removedFiles.ToArray(),
                ModifiedFiles = modifiedFiles.ToArray()
            };

            return diff;
        }
    }
}
