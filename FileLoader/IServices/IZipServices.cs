using FileLoader.Business;

namespace FileLoader.IServices
{
    public interface IZipServices
    {
        void UnzipFiles(FileManagementResult fileManagementResult, string destinationPath, string fileSeparator);
        NodeCollection GetFileAndFolderStructureAsync(string filePath);
    }
}
