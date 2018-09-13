using AxinomCommon.Business;
using AxinomCommon.IServices;
using DataManagementSystem.Business;
using DataManagementSystem.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

            // get file nodes (decrypts inside)
            var fileNodes = FileNode.ExtractFromJsonNode(_encryptionServices, tree, fileSeparator);

            // save files to local disk
            _fileManagementServices.StoreFilesAsync(fileNodes);

            // persist files
            _persistenceServices.Insert(fileNodes.Select(x => x.ToDbFileNode()));

            return Ok();
        }
    }
}
