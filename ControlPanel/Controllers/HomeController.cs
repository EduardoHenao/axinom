﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ControlPanel.Models;

namespace ControlPanel.Controllers
{
    /**
     * this class is here only to show the view for the Control panel 
     */
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
