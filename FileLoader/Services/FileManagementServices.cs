using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileLoader.Services
{
    public class FileManagementServices : IFileManagementServices
    {
        private string rootFolder;
        private readonly string storageFolder = "_uploadedFiles";
        private readonly string unzipFolder = "_unzippedFiles";

        public FileManagementServices(IHostingEnvironment env)
        {
            rootFolder = env.ContentRootPath;
        }

        public async Task<FileManagementResult> StoreFilesAsync(IFormFile file)
        {
            string destinyPath = GetFilesPath();
            string treatmentDate = DateTime.UtcNow.ToString("yyyyMMdd-Hmmss");

            FileManagementResult newFile = new FileManagementResult()
            {
                FileName = Path.Combine(destinyPath, $"{file.FileName}_{treatmentDate}.zip"),
                Length = file.Length
            };

            if (newFile.Length > 0)
            {
                using (var stream = new FileStream(newFile.FileName, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    newFile.IsStored = true;
                }
            }

            return newFile;
        }

        public string GetFilesPath()
        {
            return Path.Combine(rootFolder, storageFolder);
        }

        public string GetUnzipPath()
        {
            return Path.Combine(rootFolder, unzipFolder);
        }

        public void EnsureStoreDirectory()
        {
            EnsureDirectory(GetFilesPath());
        }

        public void EnsureUnzipDirectory()
        {
            EnsureDirectory(GetUnzipPath());  
        }

        private void EnsureDirectory(string path)
        {
            if (Directory.Exists(path)) Directory.Delete(path, true); // recursively delete directory
            Directory.CreateDirectory(path);
        }

    }

    public interface IFileManagementServices
    {
        Task<FileManagementResult> StoreFilesAsync(IFormFile file);
        string GetFilesPath();
        string GetUnzipPath();
        void EnsureStoreDirectory();
        void EnsureUnzipDirectory();
    }

    public class FileManagementResult
    {
        public long Length { get; set; } = 0;
        public bool IsStored { get; set; } = false;
        public string FileName { get; set; }
    }
}
