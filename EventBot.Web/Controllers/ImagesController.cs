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
            //TODO better way to fetch default image if id==0
            var imageBytes = _service.GetImage(id==0?6:id);

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
        [HttpPost]
        public ActionResult UploadGetId(HttpPostedFileBase file)
        {
            int id = 0;
            if (file != null && file.ContentLength > 0)
            {
                var ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                id=_service.CreateImage(ms.ToArray());
            }
            return Content(id.ToString());
        }
    }
}