using System.IO.Compression;
using FileLoader.Business;

namespace FileLoader.Services
{
    public class ZipServices : IZipServices
    {
        public ZipServices()
        {
        }

        public NodeCollection GetFileAndFolderStructureAsync(string filePath)
        {
            NodeCollection nodeCollection = new NodeCollection();

            using (ZipArchive zipFile = ZipFile.OpenRead(filePath))
            {
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {
                    nodeCollection.AddEntry(entry.FullName, 0);
                }
            }

            return nodeCollection;
        }

        public void UnzipFiles(FileManagementResult fileManagementResult, string destinationPath)
        {

            using (ZipArchive zipFile = ZipFile.OpenRead(fileManagementResult.FileName))
            {
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {
                    entry.ExtractToFile(destinationPath);
                }
            }
        }

    }

    public interface IZipServices
    {
        void UnzipFiles(FileManagementResult fileManagementResult, string destinationPath);
        NodeCollection GetFileAndFolderStructureAsync(string filePath);
    }
}
