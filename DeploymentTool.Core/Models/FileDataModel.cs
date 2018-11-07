using DeploymentTool.Core.Filesystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentTool.Core.Models
{
    public class FileDataModel : IEquatable<FileDataModel>
    {
        public string Filename { get; set; }

        public DateTime DateModified { get; set; }

        public string MD5 { get; set; }

        public static FileDataModel Calculate(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            FileInfo info = new FileInfo(filename);

            return Calculate(info);
        }

        public static FileDataModel Calculate(FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            FileDataModel result = new FileDataModel
            {
                DateModified = fileInfo.LastWriteTimeUtc,
                Filename = fileInfo.FullName,
                MD5 = FilesystemUtils.CalculateMD5(fileInfo.FullName)
            };

            return result;
        }

        public bool EqualsByHash(FileDataModel other)
        {
            if (other == null)
            {
                return false;
            }
            return MD5 == other.MD5;
        }

        public bool EqualsByFilename(FileDataModel other)
        {
            if (other == null)
            {
                return false;
            }
            return Filename == other.Filename;
        }

        public bool Equals(FileDataModel other)
        {
            return EqualsByHash(other) && EqualsByFilename(other);
        }
    }
}
