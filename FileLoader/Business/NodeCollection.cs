using System.Collections.Generic;
using System.IO;
using FileLoader.Services;

namespace FileLoader.Business
{
    public class NodeCollection : Dictionary<string, Node>
    {
        private readonly string bslash = "\\";

        public void AddEntry(string entry, int beginIndex)
        {
            if (beginIndex < entry.Length)
            {
                int endIndex = entry.IndexOf("/", beginIndex);
                if (endIndex == -1)
                {
                    endIndex = entry.Length;
                }
                string name = entry.Substring(beginIndex, endIndex - beginIndex);
                if (!string.IsNullOrEmpty(name))
                {
                    Node item;

                    if (this.ContainsKey(name))
                    {
                        item = this[name];
                    }
                    else
                    {
                        item = new Node();
                        this.Add(name, item);
                    }

                    // now add the rest to the new item's children
                    item.Children.AddEntry(entry, endIndex + 1);
                }
            }
        }

        public JsonNode GenerateJsonObject(IEncryptionServices encryptionServices, string filePath)
        {
            JsonNode root = new JsonNode();
            GenerateJsonObjectRecursive(root, filePath, this, encryptionServices, bslash);
            return root;
        }

        private void GenerateJsonObjectRecursive(JsonNode jsonNode, string filePath, NodeCollection nodeCollection, IEncryptionServices encryptionServices, string accumulatedPath)
        {
            foreach (KeyValuePair<string, Node> keyValuePair in nodeCollection)
            {
                if (keyValuePair.Value.Children.Count == 0) //is a file
                {
                    string fullFilePath = $"{filePath}{accumulatedPath}{keyValuePair.Key}";
                    byte[] array = File.ReadAllBytes(fullFilePath);
                    jsonNode.children.Add(new JsonNode(
                        name: encryptionServices.EncryptToString(keyValuePair.Key), 
                        isFile: true,
                        file: encryptionServices.EncryptToString(array) ));
                }
                else
                {
                    var dirNode = new JsonNode(name: encryptionServices.EncryptToString(keyValuePair.Key));
                    jsonNode.children.Add(dirNode);
                    GenerateJsonObjectRecursive(dirNode, filePath, keyValuePair.Value.Children, encryptionServices, $"{accumulatedPath}{keyValuePair.Key}{bslash}");
                }
            }
        }
    }
}
