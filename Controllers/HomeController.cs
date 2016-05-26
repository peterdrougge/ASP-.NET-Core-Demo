using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ASPNETCoreRC2Demo
{
    public class HomeController : Controller
    {
        private MyAppSettings _options {get;}
	
        public HomeController(IOptions<MyAppSettings> options)
        {
            _options = options.Value;
        }
        public IActionResult Index()
        {
            ViewData["aspnetcorerc2demo"] = _options.aspnetcorerc2demo;
            return View();
        }
        
        public IActionResult Error()
        {
            return View();
        }
    }
}