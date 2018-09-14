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
    public class UploadFilesController : Controller
    {
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IZipServices _zipServices;
        private readonly IEncryptionServices _encryptionServices;
        private readonly IDataManagementSystemCallerServices _dataManagementSystemCallerServices;
        private readonly IConfiguration _configuration;

        // conf and defautl conf values
        private const string _userFieldName = "User";
        private readonly string _user;
        private const string _defaultUser = "Axinom";

        private const string _passwordFieldName = "Password";
        private readonly string _password;
        private const string _defaultPassword = "Monixa";

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
            _configuration = configuration;

            
            var user = configuration[_userFieldName];
            _user = string.IsNullOrEmpty(user) ? _defaultUser : user;

            var password = configuration[_passwordFieldName];
            _password = string.IsNullOrEmpty(password) ? _defaultPassword : password;
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

                    await _dataManagementSystemCallerServices.PostAsync(jsonString, _user, _password);

                    return Ok(jsonString);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}