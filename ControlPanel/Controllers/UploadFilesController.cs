using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AxinomCommon.Business;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using ControlPanel.IServices;
using AxinomCommon.IServices;
using ControlPanel.Models;

namespace ControlPanel.Controllers
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

        private const string _fileManagementServiceUrlFieldName = "FileManagementServiceUrl";
        private const string _defaultFileManagementServiceUrl = "http://localhost:5000";
        private readonly string _fileManagementServiceUrl;

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

            //get constants from conf
            var defaultFileManagementServiceUrl = configuration[_fileManagementServiceUrlFieldName];
            _fileManagementServiceUrl = string.IsNullOrEmpty(defaultFileManagementServiceUrl) ? _defaultFileManagementServiceUrl : defaultFileManagementServiceUrl;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile formFile, string user, string password)
        {
            if (formFile == null || string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                return View(new PostModel { ErrorMessage = "Please provide a correct user, password and zip file", HasError = true });
            }
            else
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
                    var response = await _dataManagementSystemCallerServices.PostAsync(
                        _fileManagementServiceUrl,
                        jsonString,
                        _encryptionServices.EncryptToString(user),
                        _encryptionServices.EncryptToString(password));

                    // visualized the sent json string
                    if (!response) return View(new PostModel { ErrorMessage = "The Data Management system answer was negative", HasError = true });

                    return View(new PostModel { JsonString = jsonString, AnswerComment = "Json accepted by Data Management Service", HasError = false });
                }
                else
                {
                    return View(new PostModel { ErrorMessage = "Error Storing zip file", HasError = true });
                }
            }
        }
    }
}