using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILoggerFactory _Factory;

        public HomeController(ILoggerFactory loggerFactory)
        {
            _Factory = loggerFactory;
        }

        public IActionResult Index()
        {
            log("Home : Index");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void log(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            var logger = _Factory.CreateLogger(nameof(HomeController));
            logger.LogDebug(text);
        }
    }
}
