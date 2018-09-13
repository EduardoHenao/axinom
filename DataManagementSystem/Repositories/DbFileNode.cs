using System;

namespace DataManagementSystem.Repositories
{
    public class DbFileNode
    {
        public long Id { get; set; }
        public string RelativePath { get; set; }
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
        public DateTime DateTime { get; set; }
    }
}
