using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HandsToOfferApi.Dto;
using HandsToOfferApi.Models;

namespace HandsToOfferApi.Controllers
{
    public class HomeController : Controller
    {
        private H2OContext db = new H2OContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Project()
        {
            List<Project> project = db.Project.OrderByDescending(x=>x.UpdatedDate).ToList();
            return View(project);
        }

        public ActionResult Event(string message)
        {
            List<Event> events = db.Event.OrderByDescending(x => x.UpdatedDate).ToList();
            ViewBag.message = message;
            return View(events);
        }

        public ActionResult AddEvent()
        {
            return View();
        }

       // public ActionResult CreateEvent()
       // {
       //     return View();
       // }

        public ActionResult CreateEvent(int? eventId)
        {
            if (eventId ==null || eventId == 0)
            {
                return View();
            }
            else {
                Event evenT = db.Event.Where(x => x.EventId == eventId).FirstOrDefault();
                return View(evenT);
            }

        }

        [HttpPost]
        public ActionResult CreateEvent(FormCollection collection)
        {
            var model = new Event();
            TryUpdateModel(model, collection);
            model.UpdatedDate = DateTime.UtcNow;
            db.Event.Add(model);
            db.SaveChanges();
            return RedirectToAction("Event",new {message = model.ProjectName + "Event has been created Successfully" });
        }

        public ActionResult EventSuccess()
        {
            return View();
        }

        //[WebMethod]
        public JsonResult ProjectUpdate (ProjectOperation finalProject )
        {
            Project project = finalProject.project;
            string operation = finalProject.operation;
            Project projectU = new Project();
            bool changed = false;
            project.UpdatedDate = DateTime.UtcNow;
            try
            {
                if (operation == "a")
                {
                    db.Project.Add(project);
                }
                else if (operation == "u")
                {
                    projectU = db.Project.Where(x => x.ProjectId == project.ProjectId).FirstOrDefault();
                    projectU.ProjectName = project.ProjectName;
                    projectU.ProjectDesc = project.ProjectDesc;
                    projectU.UpdatedDate = DateTime.Now;
                }
                else {
                    //projectU.UpdatedDate = DateTime.Now;
                    //project.IsActive = false;
                    db.Project.Attach(project);
                    db.Project.Remove(project);                
                }
                changed = true;
                db.SaveChanges();
                return Json(changed, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(changed, JsonRequestBehavior.AllowGet);
            }
        }

    }
}