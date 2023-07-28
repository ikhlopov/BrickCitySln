using BrickCity.Data;
using BrickCity.Models;
using BrickCity.Models.EFEntity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrickCity.Controllers
{
    public class PredictController : Controller
    {
        // Контроллер вкладки "Прогноз"

        private ILogger<HomeController> _logger;
        private BrickCityContext _context;
        public PredictController(BrickCityContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
            _logger.LogDebug(1, "NLog injected into HomeController");
        }

        [HttpGet]
        public IActionResult Predict()
        {

            List<FileModel> files = _context.Files.ToList();
            ViewBag.files = files;

            var file = _context.Files.AsEnumerable().MaxBy(f => f.Date);
            var predictDate = file.MaxRecordDate;
            ViewBag.predictDate = predictDate;
            PredictModel model = new PredictModel(file, predictDate);
            return View(model);
        }
        [HttpPost]
        public IActionResult Predict(DateTime predictDate, int FileID)
        {
            List<FileModel> files = _context.Files.ToList();
            ViewBag.files = files;
            ViewBag.predictDate = predictDate;
            var file = _context.Files.Where(f => f.FileModelID == FileID).Single();
            PredictModel model = new PredictModel(file, predictDate);
            return View(model);
        }
    }
}
