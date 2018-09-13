using AxinomCommon.Business;
using AxinomCommon.IServices;
using DataManagementSystem.Business;
using DataManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DataManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ReceiveController : ControllerBase
    {
        private readonly IEncryptionServices _encryptionServices;
        private readonly IZipServices _zipServices;
        private readonly IFileManagementServices _fileManagementServices;
        private readonly IPersistenceServices _persistenceServices;

        public ReceiveController(
            IEncryptionServices encryptionServices, 
            IZipServices zipServices, 
            IFileManagementServices fileManagementServices, 
            IPersistenceServices persistenceServices)
        {
            _encryptionServices = encryptionServices;
            _zipServices = zipServices;
            _fileManagementServices = fileManagementServices;
            _persistenceServices = persistenceServices;
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] JsonNode tree)
        {
            if (tree == null) return BadRequest();

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
            _persistenceServices.Insert(dbFileNodes);

            return Ok();
        }
    }
}
