using BrickCity.Data;
using BrickCity.Models;
using BrickCity.Models.EFEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrickCity.Controllers
{
    public class TrendController : Controller
    {
        // Контроллер вкладки "Тренд"
        private readonly BrickCityContext _context;
        public TrendController(BrickCityContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Trend()
        {
            List<FileModel> files = _context.Files
                .ToList();
            ViewBag.files = files;

            TrendGraph graph = new()
            {
                SelectedFile = _context.Files.ToList().MaxBy(f => f.Date)
            };
            return View(graph);
        }
        [HttpPost]
        public ActionResult Trend(int FileID)
        {
            List<FileModel> files = _context.Files.ToList();
            ViewBag.files = files;

            var model = new TrendGraph { SelectedFile = _context.Files.Single(f => f.FileModelID == FileID) };


            return View(model);
        }
    }
}
