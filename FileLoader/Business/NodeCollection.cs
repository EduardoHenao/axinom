using System.Collections.Generic;
using FileLoader.Services;

namespace FileLoader.Business
{
    public class NodeCollection : Dictionary<string, Node>
    {
        public void AddEntry(string entry, int beginIndex)
        {
            if (beginIndex < entry.Length)
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

        public string GenerateJson(IEncryptionServices encryptionServices)
        {
            string result = string.Empty;
            foreach (var key in Keys)
            {
                var value = this[key];
            }

            return result;
        }
    }
}
