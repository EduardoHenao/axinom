using FileLoader.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FileLoader.Controllers
{
    public class UploadFilesController : Controller
    {
        private readonly IFileManagementServices fileManagementServices;
        private readonly IZipServices zipServices;

        public UploadFilesController(IFileManagementServices fileManagementServices, IZipServices zipServices)
        {
            this.fileManagementServices = fileManagementServices;
            this.zipServices = zipServices;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(IFormFile formFile)
        {
            FileManagementResult storedFile = await fileManagementServices.StoreFilesAsync(formFile);

            NodeCollection nodes = zipServices.GetFileAndFolderStructureAsync(storedFile.FileName);

            // encrypt filetree  ???

            return Ok(nodes);
        }
    }
}