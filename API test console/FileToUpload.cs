namespace API_TEST_CONSOLE
{
    public class FileToUpload
    {
        public FileToUpload(string localFilename, string targetRelativeFilename)
        {
            LocalFilename = localFilename;
            TargetRelativeFilename = targetRelativeFilename;
        }

        public string TargetRelativeFilename { get; set; }
        public string LocalFilename { get; set; }
    }
}
