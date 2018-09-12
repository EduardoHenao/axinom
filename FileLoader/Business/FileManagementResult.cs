namespace FileLoader.Business
{
    public class FileManagementResult
    {
        public long Length { get; set; } = 0;
        public bool IsStored { get; set; } = false;
        public string FileName { get; set; }
    }
}
