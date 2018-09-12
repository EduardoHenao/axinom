using FileLoader.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileLoader.Business;
using Newtonsoft.Json;

namespace FileLoader.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IZipServices _zipServices;
        private readonly IEncryptionServices _encryptionServices;

        public UploadFilesController(IFileManagementServices fileManagementServices, IZipServices zipServices, IEncryptionServices encryptionServices)
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
                FileManagementResult storedFile = await _fileManagementServices.StoreFilesAsync(formFile);

                if (storedFile.IsStored)
                {
                    NodeCollection nodes = _zipServices.GetFileAndFolderStructureAsync(storedFile.FileName);
                    string destinyPath = _fileManagementServices.GetFilesPath();
                    JsonNode root = nodes.GenerateJsonObject(_encryptionServices, destinyPath);
                    string json = JsonConvert.SerializeObject(root);

                    return Ok(json);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}