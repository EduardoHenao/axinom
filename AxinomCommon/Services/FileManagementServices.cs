using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using AxinomCommon.Business;
using AxinomCommon.IServices;
using System.Collections.Generic;
using DataManagementSystem.Business;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace AxinomCommon.Services
{
    public class FileManagementServices : IFileManagementServices
    {
        private readonly string _rootFolder;

        private readonly string _storageFolder;
        private const string _defaultStorageFolder = "_uploadedFiles";

        private readonly string _unzipFolder;
        private const string _defaultUnzipFolder = "_unzippedFiles";

        private readonly string _destinyFolder;
        private const string _defaultDestinyFolder = "_destinyFiles";

        private readonly string _fileSeparator;
        private const string _defaultFileSeparator = "\\";

        private readonly ILogger _logger;

        public FileManagementServices(IHostingEnvironment env, IConfiguration configuration, ILogger<FileManagementServices> logger)
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

            var destinyFolder = configuration["DestinyFolder"];
            _destinyFolder = string.IsNullOrEmpty(destinyFolder) ? _defaultDestinyFolder : destinyFolder;

            _rootFolder = env.ContentRootPath;

            _logger = logger;
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

        public void StoreFilesAsync(IEnumerable<FileNode> fileNodes)
        {            
            foreach (var fileNode in fileNodes)
            {
                if (string.IsNullOrEmpty(_rootFolder)) _logger.LogError("root folder is undefined");
                if (string.IsNullOrEmpty(fileNode.FileName)) _logger.LogError("warning there is an empty filename, this shouldnt happen");
                if (string.IsNullOrEmpty(fileNode.RelativePath)) _logger.LogError($"warning there is an empty path for file {fileNode.FileName}, this shouldnt happen");
                if (fileNode.FileBytes.Length == 0) _logger.LogError($"warning there is an empty file content for {fileNode.FileName}, this shouldnt happen");

                string treatmentDate = DateTime.UtcNow.ToString("yyyyMMdd-Hmmss"); // as in ControlPanel with the zips, store in a dedicated folder based on date
                string fullpath = $"{_rootFolder}{_fileSeparator}{_destinyFolder}{_fileSeparator}{treatmentDate}{fileNode.RelativePath}";
                Directory.CreateDirectory(fullpath);
                File.WriteAllBytes($"{fullpath}{fileNode.FileName}", fileNode.FileBytes);
            }
        }

        public string GetFilesPath()
        {
            return Path.Combine(_rootFolder, _storageFolder);
        }

        public string GetUnzipPath()
        {
            return Path.Combine(_rootFolder, _unzipFolder);
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

        private void EnsureDirectory(string path, bool recursive = true)
        {
            if (Directory.Exists(path)) Directory.Delete(path, recursive); // recursively delete directory
            Directory.CreateDirectory(path);
        }
    }
}
