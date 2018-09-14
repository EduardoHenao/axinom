using System.Collections.Generic;

namespace AxinomCommon.Business
{
    /*
     * This class is common to both the Control Panel and the File Management System
     * It represents the json containing the tree structure  to be coded and decoded in both sides.
     */
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
