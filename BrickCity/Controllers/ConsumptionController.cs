using BrickCity.Models;
using Highsoft.Web.Mvc.Charts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using BrickCity.Data;
using Microsoft.EntityFrameworkCore;
using BrickCity.Models.EFEntity;

namespace BrickCity.Controllers
{
    public class ConsumptionController : Controller
    {
        // Контроллер вкладки "Потребление"

        private readonly BrickCityContext _context;

        public ConsumptionController(BrickCityContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Consumption()
        {
            List<FileModel> files = _context.Files.ToList();
            ViewBag.files = files;
            return View(new ConsumptionAreaGraph());
        }

        [HttpPost]
        public ActionResult Consumption(DateTime fromDate , DateTime toDate, int FileID)
        {
            var selectedFile = _context.Files.FirstOrDefault(f => f.FileModelID == FileID);
            var consumers = _context.Consumers.ToList().Where(c => c.File == selectedFile);
            
            foreach (var consumer in consumers)
            {
                _context.Entry(consumer)
                    .Collection(b => b.Consumptions)
                    .Load();
            }

            List<FileModel> files = _context.Files.ToList();
            ViewBag.files = files;

            var graph = new ConsumptionAreaGraph();
            graph.SelectedFile = selectedFile;
            graph.FromDate = fromDate; 
            graph.ToDate = toDate;
            
            graph.Consumers = consumers;
            return View(graph);
        }
    }
}