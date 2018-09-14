using System.Collections.Generic;
using AxinomCommon.Business;
using AxinomCommon.IServices;

namespace DataManagementSystem.Business
{
    /* 
     * this class is used in the Data Management system.
     * The data management system will parse the json into a JsonNode containing the encrypted tree structure.
     * This class will take the JsonNode tree object and some other information to generate the actual
     * decrypted file structure inside the data management system (represented by this class)
     * 
     * JsonNode.Folder "/"
     * |
     * |->JsonNode.Folder "c"
     * |           |
     * |           |- JsonNode.File "b.txt"
     * |
     * |- JsonNode.File "a.txt"
     * 
     * will convert to:
     * 
     * Filenode 1: /c/b.txt
     * Filenode 2: /a.txt
     */
    public class FileNode
    {
        public string RelativePath { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }

        public static List<FileNode> ExtractFromJsonNode(
            IEncryptionServices encryptionServices, 
            JsonNode jsonNode, 
            string fileSeparator)
        {
            List<FileNode> answer = new List<FileNode>();
            ExtractFromJsonNodeRecursive(encryptionServices, answer, jsonNode, string.Empty, fileSeparator);
            return answer;
        }

        private static void ExtractFromJsonNodeRecursive(
            IEncryptionServices encryptionServices, 
            List<FileNode> fileNodesList, 
            JsonNode jsonNodeParent, 
            string accumulatedPath, 
            string fileSeparator)
        {
            if (jsonNodeParent.isFile) //is a file node
            {
                fileNodesList.Add(new FileNode
                {
                    RelativePath = accumulatedPath,
                    FileName = encryptionServices.DecryptToString(jsonNodeParent.name),
                    FileBytes = encryptionServices.DecryptToBytes(jsonNodeParent.file)
                });
            }
            else // this is a directory.
            {
                foreach (var jsonNodeChild in jsonNodeParent.children)
                {
                    ExtractFromJsonNodeRecursive(
                        encryptionServices,
                        fileNodesList, 
                        jsonNodeChild, 
                        $"{accumulatedPath}{encryptionServices.DecryptToString(jsonNodeParent.name)}{fileSeparator}", 
                        fileSeparator);
                }
            }
        }
    }
}
