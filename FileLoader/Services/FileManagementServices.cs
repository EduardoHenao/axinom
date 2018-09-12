using FileLoader.Business;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileLoader.Services
{
    public class FileManagementServices : IFileManagementServices
    {
        private string rootFolder;

        private readonly string _storageFolder;
        private const string _defaultStorageFolder = "_uploadedFiles";

        private readonly string _unzipFolder;
        private const string _defaultUnzipFolder = "_unzippedFiles";

        private readonly string _fileSeparator;
        private const string _defaultFileSeparator = "\\";

        public FileManagementServices(IHostingEnvironment env, IConfiguration configuration)
        {
            //file separator
            var fileSeparator = configuration["FileSeparator"];
            _fileSeparator = string.IsNullOrEmpty(fileSeparator) ? _defaultFileSeparator : fileSeparator; // if  not in conf, defautl 'default key'

            //uploaded files directory
            var storageFolder = configuration["StorageFolder"];
            _storageFolder = string.IsNullOrEmpty(storageFolder) ? _defaultStorageFolder : storageFolder;

            //unzip files directory
            var unzipFolder = configuration["UnzipFolder"];
            _unzipFolder = string.IsNullOrEmpty(unzipFolder) ? _defaultUnzipFolder : unzipFolder;

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
            return Path.Combine(rootFolder, _storageFolder);
        }

        public string GetUnzipPath()
        {
            return Path.Combine(rootFolder, _unzipFolder);
        }

        public string GetFileSeparator()
        {
            return _fileSeparator;
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
        string GetFileSeparator();
        void EnsureStoreDirectory();
        void EnsureUnzipDirectory();
    }
}
