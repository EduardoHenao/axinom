using DataManagementSystem.Business;
using DataManagementSystem.Repositories;
using System;

namespace AxinomCommon.Business
{
    public static class FileNodeExtensions
    {
        // in this case my persistancy layer is above the project containing the file node
        // i'll code an extension method to convert a AxinomCommon.Business.FileNode into DataManagementSystem.Repositories.DbFileNode
        public static DbFileNode ToDbFileNode(this FileNode fileNode)
        {
            return new DbFileNode
            {
                DateTime = DateTime.UtcNow,
                FileBytes = fileNode.FileBytes,
                FileName = fileNode.FileName,
                RelativePath = fileNode.RelativePath
            };
        }
    }
}
