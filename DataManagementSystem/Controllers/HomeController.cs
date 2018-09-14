using Microsoft.AspNetCore.Mvc;

namespace DataManagementSystem.Controllers
{
    /*
     * Just returns a view to show the Data Management System instance is alive
     */
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}