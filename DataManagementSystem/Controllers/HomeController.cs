using Microsoft.AspNetCore.Mvc;

namespace DataManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}