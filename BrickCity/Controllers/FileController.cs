using BrickCity.Data;
using BrickCity.Models.EFEntity;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Web;

namespace BrickCity.Controllers
{
    public class FileController : Controller
    {
        // Контроллер вкладки "Файл"
        private BrickCityContext _context;

        public FileController(BrickCityContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<FileModel> FileList = _context.Files.ToList<FileModel>();
            return View("File", FileList);
        }

        [HttpPost]
        public ActionResult Upload(IFormFile upload, string filename)
        {
            if(upload != null)
            {
                var newFile = AddFile(_context, FileStringFromUpload(upload), filename);
                DbInitializer.AddConsumersFromFileToContext(newFile, _context);
            }
            return RedirectToAction("Index");
        }

        private string FileStringFromUpload(IFormFile upload) 
        {
            string result;
            using (MemoryStream ms = new MemoryStream())
            {
                upload.CopyTo(ms);
                result = Encoding.UTF8.GetString(ms.ToArray());
            }
            return result;
        }

        private FileModel AddFile(BrickCityContext context, string content, string fileName)
        {
            FileModel newFile = new FileModel {
                Date = DateTime.Now,
                Name = fileName,
                Content = content

            };
            context.Files.Add(newFile);
            context.SaveChanges();
            return newFile;
        }
    }
}
