using FileLoader.Business;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace FileLoader.IServices
{
    public interface IFileManagementServices
    {
        Task<FileManagementResult> StoreFilesAsync(IFormFile file);
        string GetFilesPath();
        string GetUnzipPath();
        string GetFileSeparator();
        void EnsureStoreDirectory();
        void EnsureUnzipDirectory();
    }
}
