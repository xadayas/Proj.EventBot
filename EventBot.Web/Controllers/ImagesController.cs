using System.IO;
using System.Web;
using System.Web.Mvc;
using EventBot.Entities.Service;
using EventBot.Entities.Service.Interfaces;

namespace EventBot.Web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IEventService _service = new EventServiceEf();

        // GET: Images
        public void View(int id)
        {
            var imageBytes = _service.GetImage(id);

            var ms = new MemoryStream(imageBytes);
            Response.ContentType = "Image/Png";
            ms.CopyTo(Response.OutputStream);
        }

        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                _service.CreateImage(ms.ToArray());
            }
            return View();
        }
    }
}