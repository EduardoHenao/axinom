namespace AxinomCommon.Business
{
    /*
     * class that contains information about the file information process
     */
    public class FileManagementResult
    {
        public long Length { get; set; } = 0;
        public bool IsStored { get; set; } = false;
        public string FileName { get; set; }
    }
}
