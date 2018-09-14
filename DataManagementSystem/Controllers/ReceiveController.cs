using AxinomCommon.Business;
using AxinomCommon.IServices;
using DataManagementSystem.Business;
using DataManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using DataManagementSystem.Filters;
using Microsoft.Extensions.Configuration;

namespace DataManagementSystem.Controllers
{
    /*
     * By requirement: and JSON API Controller with 1 call that accepts the encrypted tree file structure
     * 
     * if you understand the post method, you understood the whole Data Management System
     */
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ReceiveController : ControllerBase
    {
        private readonly IEncryptionServices _encryptionServices;
        private readonly IZipServices _zipServices;
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IPersistenceServices _persistenceServices;

        private const string _persistFilesFieldName = "PersistFiles";
        private readonly bool _persistFiles;
        private const string _defaultPersistFiles = "False";

        private readonly IConfiguration _configuration;

        public ReceiveController(
            IEncryptionServices encryptionServices, 
            IZipServices zipServices, 
            IFileManagementServices fileManagementServices, 
            IPersistenceServices persistenceServices,
            IConfiguration configuration)
        {
            _encryptionServices = encryptionServices;
            _zipServices = zipServices;
            _fileManagementServices = fileManagementServices;
            _persistenceServices = persistenceServices;
            _configuration = configuration;

            var persistFiles = configuration[_persistFilesFieldName];
            persistFiles = string.IsNullOrEmpty(persistFiles) ? _defaultPersistFiles : persistFiles; // if  not in conf, defautl 'default key'
            _persistFiles = bool.Parse(persistFiles);
        }

        // POST api/values
        [HttpPost]
        [TypeFilter(typeof(VerifyAuthFilter), IsReusable = true)]
        public IActionResult Post([FromBody] JsonNode tree, bool authorized)
        {
            if (tree == null || !authorized) return BadRequest();

            // gather variables
            var fileSeparator = _fileManagementServices.GetFileSeparator();

            // get file nodes (by processing json and decrypting encrypted fields)
            var fileNodes = FileNode.ExtractFromJsonNode(_encryptionServices, tree, fileSeparator);

            // save files to local disk
            string treatmentDate = DateTime.UtcNow.ToString("yyyyMMdd-Hmmss");
            _fileManagementServices.StoreFilesAsync(fileNodes, treatmentDate);

            //convert FileNodes to DbFileNodes
            var dbFileNodes = fileNodes.Select(x => x.ToDbFileNode(treatmentDate, _fileManagementServices.GetFileSeparator()));

            //inject treatment date folder (see how the above call _fileManagementServices.StoreFilesAsync works)
            dbFileNodes.Select(x => x.InjectTreatmentDateToRelativePath(treatmentDate, _fileManagementServices.GetFileSeparator()));

            // persist files in db
            if(_persistFiles) _persistenceServices.Insert(dbFileNodes);

            return Ok();
        }
    }
}
