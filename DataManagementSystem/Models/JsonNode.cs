using System.Collections.Generic;

namespace DataManagementSystem.Models
{
    public class JsonNode
    {
        public string name { get; set; }
        public bool isFile { get; set; }
        public List<JsonNode> children { get; set; }
        public string file { get; set; }
    }
}
