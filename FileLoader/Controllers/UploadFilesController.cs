using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AxinomCommon.Business;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using FileLoader.IServices;
using AxinomCommon.IServices;

namespace FileLoader.Controllers
{
    /*
     * Class to handle the data from the view containing the user password and file
     * all the important stuff for the Control panel happens in the post action
     */
    public class UploadFilesController : Controller
    {
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IZipServices _zipServices;
        private readonly IEncryptionServices _encryptionServices;
        private readonly IDataManagementSystemCallerServices _dataManagementSystemCallerServices;

        public UploadFilesController(
            IFileManagementServices fileManagementServices, 
            IZipServices zipServices, 
            IEncryptionServices encryptionServices,
            IDataManagementSystemCallerServices dataManagementSystemCallerServices,
            IConfiguration configuration)
        {
            _fileManagementServices = fileManagementServices;
            _zipServices = zipServices;
            _encryptionServices = encryptionServices;
            _dataManagementSystemCallerServices = dataManagementSystemCallerServices;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile formFile, string user, string password)
        {
            if (formFile != null && !string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password))
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

                    // this will generate the json object with all neccessary fields encrypted
                    JsonNode root = nodes.GenerateJsonObject(
                        _encryptionServices, 
                        _fileManagementServices.GetUnzipPath(), 
                        _fileManagementServices.GetFileSeparator());

                    // then generate the json string
                    string jsonString = JsonConvert.SerializeObject(root);

                    // call the DataManagement System API
                    await _dataManagementSystemCallerServices.PostAsync(
                        "http://localhost:5000",
                        jsonString,
                        _encryptionServices.EncryptToString(user),
                        _encryptionServices.EncryptToString(password));

                    // visualized the sent json string
                    return Ok(jsonString);
                }
                else
                {
                    //todo: maybe return a partial view with information?
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}