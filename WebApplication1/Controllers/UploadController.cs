using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class UploadController : Controller
    {
        private readonly ILoggerFactory _Factory;

        public UploadController(ILoggerFactory loggerFactory)
        {
            _Factory = loggerFactory;
        }

        //[HttpPost("UploadFile")]
        //public async Task<IActionResult> Post(List<IFormFile> files)
        //{
        //    if (files == null)
        //    {
        //        log("the file is empty.");
        //    }

        //    //get a new file path
        //    //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.GetRandomFileName());

        //    //using (var stream = new FileStream(path, FileMode.Create))
        //    //{
        //    //    await file.CopyToAsync(stream);
        //    //}

        //    log("Upload completed");
        //    return RedirectToRoute("Home");
        //}

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            // full path to file in temp location
            var filePath = Path.GetTempFileName();

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size, filePath });
        }


        public void log(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            var logger = _Factory.CreateLogger(nameof(UploadController));
            logger.LogDebug(text);
        }
    }
}
