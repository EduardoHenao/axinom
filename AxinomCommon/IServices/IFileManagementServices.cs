using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AxinomCommon.Business;
using System.Collections.Generic;
using DataManagementSystem.Business;

namespace AxinomCommon.IServices
{
    public interface IFileManagementServices
    {
        Task<FileManagementResult> StoreFilesAsync(IFormFile file);
        void StoreFilesAsync(IEnumerable<FileNode> fileNodes);
        string GetFilesPath();
        string GetUnzipPath();
        string GetFileSeparator();
        void EnsureStoreDirectory();
        void EnsureUnzipDirectory();
    }
}
