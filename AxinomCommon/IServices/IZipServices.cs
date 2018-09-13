using AxinomCommon.Business;

namespace AxinomCommon.IServices
{
    public interface IZipServices
    {
        void UnzipFiles(FileManagementResult fileManagementResult, string destinationPath, string fileSeparator);
        NodeCollection GetFileAndFolderStructureAsync(string filePath);
    }
}
