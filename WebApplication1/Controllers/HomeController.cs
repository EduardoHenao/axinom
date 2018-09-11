using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerFactory _Factory;

        public HomeController(ILoggerFactory loggerFactory)
        {
            _Factory = loggerFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            log("Home : Index");
            return View();
        }

        public void log(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            var logger = _Factory.CreateLogger(nameof(HomeController));
            logger.LogDebug(text);
        }
    }
}
