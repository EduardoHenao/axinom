using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IO;
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

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                log("the file is empty.");
            }

            //get a new file path
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", Path.GetRandomFileName());

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            log("Upload completed");
            return RedirectToRoute("Home");
        }

        public void log(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            var logger = _Factory.CreateLogger(nameof(UploadController));
            logger.LogDebug(text);
        }
    }
}
