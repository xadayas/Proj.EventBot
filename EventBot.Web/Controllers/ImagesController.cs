using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using EventBot.Entities.Models;
using EventBot.Entities.Service;
using EventBot.Entities.Service.Interfaces;
using EventBot.Entities.Service.Models;
using EventBot.Web.Models;

namespace EventBot.Web.Controllers
{
    public class ImagesController : Controller
    {
        private readonly IEventService _service = new EventServiceEf();

        // GET: Images
        public void View(int id)
        {
            //TODO better way to fetch default image if id==0
            var imageBytes = _service.GetImage(id == 0 ? 6 : id);

            var ms = new MemoryStream(imageBytes);
            Response.ContentType = "Image/Png";
            ms.CopyTo(Response.OutputStream);
        }



        //public ActionResult Upload(string title, string description, string meetingPlace, string startDate, string endDate, int returnto)
        //{
        //    DateTime parsedStartDate, parsedEndDate;
        //    DateTime.TryParse(startDate, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces,
        //        out parsedStartDate);
        //    DateTime.TryParse(endDate, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces,
        //        out parsedEndDate);
        //    if (returnto != 0) Session["ReturnToEditId"] = returnto;
        //    Session["imageUploadEventSave"] = new EventViewModel { Title = title, Description = description, Location = new LocationViewModel { Name = meetingPlace }, StartDate = parsedStartDate, EndDate = parsedEndDate };
        //    return View();
        //}

        public ActionResult Upload(EventViewModel model)
        {
            
            if(model.Id!=0) Session["ReturnToEditId"] = model.Id;
            Session["imageUploadEventSave"] = model;
            return View();
        }


        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                var imageId = _service.CreateImage(ms.ToArray());
                //if (Session["ReturnToEditId"] is int)
                //{
                //    //Session["imageUploadEventSave"] = null;
                //    Session["EditImageId"] = imageId;
                //    var eventId = (int)Session["ReturnToEditId"];
                //    Session["ReturnToEditId"] = null;
                //    return RedirectToAction("Edit", "Event",new {id=eventId});
                //}
                var model = Session["imageUploadEventSave"] as EventViewModel;
                if (model != null)
                {
                    model.ImageId = imageId;
                    if (model.Id != 0) return RedirectToAction("Edit", "Event", new {id = model.Id});
                }
            }
            return RedirectToAction("Create", "Event");
            //return View();
        }
        [HttpPost]
        public ActionResult UploadGetId(HttpPostedFileBase file)
        {
            int id = 0;
            if (file != null && file.ContentLength > 0)
            {
                var ms = new MemoryStream();
                file.InputStream.CopyTo(ms);
                id = _service.CreateImage(ms.ToArray());
            }
            return Content(id.ToString());
        }
    }
}