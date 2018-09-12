using System.Collections.Generic;

namespace DataManagementSystem.Models
{
    public class JsonNode
    {
        public string name { get; set; }
        public bool isFile { get; set; }
        public List<JsonNode> children { get; }
        public string file { get; set; }

        public JsonNode(string name = "", bool isFile = false, string file = "")
        {
            this.name = name;
            this.isFile = isFile;
            this.file = file;
            children = new List<JsonNode>();
        }
    }
}
