using System.Collections.Generic;
using System.IO.Compression;

namespace FileLoader.Services
{
    public class ZipServices : IZipServices
    {
        private readonly IEncryptionServices encryptionServices;

        public ZipServices(IEncryptionServices encryptionServices)
        {
            this.encryptionServices = encryptionServices;
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
    }

    public interface IZipServices
    {
        NodeCollection GetFileAndFolderStructureAsync(string filePath);
    }

    public class Node
    {
        public Node()
        {
            this.Children = new NodeCollection();
        }

        public NodeCollection Children { get; set; }
    }

    public class NodeCollection : Dictionary<string, Node>
    {
        public void AddEntry(string entry, int beginIndex)
        {
            if(beginIndex < entry.Length)
            {
                string name;
                int endIndex;

                endIndex = entry.IndexOf("/", beginIndex);
                if (endIndex == -1)
                {
                    endIndex = entry.Length;
                }
                name = entry.Substring(beginIndex, endIndex - beginIndex);
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
    }
}
