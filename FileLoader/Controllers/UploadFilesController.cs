using FileLoader.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileLoader.Business;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace FileLoader.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IZipServices _zipServices;
        private readonly IEncryptionServices _encryptionServices;

        public UploadFilesController(IConfiguration configuration, IFileManagementServices fileManagementServices, IZipServices zipServices, IEncryptionServices encryptionServices)
        {
            _fileManagementServices = fileManagementServices;
            _zipServices = zipServices;
            _encryptionServices = encryptionServices;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile formFile)
        {
            if (formFile != null)
            {
                //ensure store directory
                _fileManagementServices.EnsureStoreDirectory();

                //delete zip files directory and recreate
                _fileManagementServices.EnsureUnzipDirectory();

                //zip storage
                FileManagementResult storedFile = await _fileManagementServices.StoreFilesAsync(formFile);

                //unzip file into zip directory
                _zipServices.UnzipFiles(
                    storedFile, 
                    _fileManagementServices.GetUnzipPath(), 
                    _fileManagementServices.GetFileSeparator());

                if (storedFile.IsStored)
                {
                    NodeCollection nodes = _zipServices.GetFileAndFolderStructureAsync(storedFile.FileName);
                    JsonNode root = nodes.GenerateJsonObject(
                        _encryptionServices, 
                        _fileManagementServices.GetUnzipPath(), 
                        _fileManagementServices.GetFileSeparator());
                    string json = JsonConvert.SerializeObject(root);

                    return Ok(json);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}