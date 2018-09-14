using System.IO;
using System.IO.Compression;
using AxinomCommon.Business;
using AxinomCommon.IServices;

namespace AxinomCommon.Services
{
    /*
     * This class is used by the control panel to handle zip files
     */
    public class ZipServices : IZipServices
    {
        /*
         * gets a path and generates the node collection (the first kind of node in the chain)
         */
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

        /*
         * method to unzip a zip file
         */
        public void UnzipFiles(FileManagementResult fileManagementResult, string destinationPath, string fileSeparator)
        {

            using (ZipArchive zipFile = ZipFile.OpenRead(fileManagementResult.FileName))
            {
                foreach (ZipArchiveEntry entry in zipFile.Entries)
                {
                    var filePath = $"{destinationPath}{fileSeparator}{FileManagementServices.CorrectFileSeparator(entry.FullName, fileSeparator)}";
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
    }
}
