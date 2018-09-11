using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public IActionResult UploadZip(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                log("the file is empty.");
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
