using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileLoader.Business;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using FileLoader.IServices;

namespace FileLoader.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IZipServices _zipServices;
        private readonly IEncryptionServices _encryptionServices;
        private readonly IDataManagementSystemCallerServices _dataManagementSystemCallerServices;

        public UploadFilesController(
            IConfiguration configuration, 
            IFileManagementServices fileManagementServices, 
            IZipServices zipServices, 
            IEncryptionServices encryptionServices,
            IDataManagementSystemCallerServices dataManagementSystemCallerServices)
        {
            _fileManagementServices = fileManagementServices;
            _zipServices = zipServices;
            _encryptionServices = encryptionServices;
            _dataManagementSystemCallerServices = dataManagementSystemCallerServices;
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
                    string jsonString = JsonConvert.SerializeObject(root);

                    var result = await _dataManagementSystemCallerServices.PostAsync(jsonString);

                    return Ok(jsonString);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}