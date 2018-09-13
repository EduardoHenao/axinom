using System.IO;
using System.IO.Compression;
using AxinomCommon.Business;
using AxinomCommon.IServices;

namespace AxinomCommon.Services
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

        public void UnzipFiles(FileManagementResult fileManagementResult, string destinationPath, string fileSeparator)
        {

            using (ZipArchive zipFile = ZipFile.OpenRead(fileManagementResult.FileName))
            {
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {
                    var filePath = $"{destinationPath}{fileSeparator}{CorrectFileSeparator(entry.FullName, fileSeparator)}";
                    var lastIndexOfFileSeparator = filePath.LastIndexOf(fileSeparator);
                    if (lastIndexOfFileSeparator != -1)
                    {
                        var folderPath = filePath.Substring(0, lastIndexOfFileSeparator);
                        Directory.CreateDirectory(folderPath);
                    }
                    entry.ExtractToFile(filePath);
                }
            }
        }

        private string CorrectFileSeparator(string fullPath, string fileSeparator)
        {
            if (fileSeparator == "/")
            {
                return fullPath.Replace("\\", "/");
            }

            return fullPath.Replace("/", "\\");
        }

    }
}
