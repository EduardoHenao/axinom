using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AxinomCommon.Business;

namespace AxinomCommon.IServices
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
