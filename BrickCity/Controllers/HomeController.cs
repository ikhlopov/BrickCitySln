using BrickCity.Models;
using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BrickCity.Data;
using Microsoft.EntityFrameworkCore;

namespace BrickCity.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BrickCityContext _context;

        public HomeController(BrickCityContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}