using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HandsToOfferApi.Common;
using HandsToOfferApi.Dto;
using HandsToOfferApi.Models;

namespace HandsToOfferApi.Controllers
{
    public class HomeController : Controller
    {
        private H2OContext db = new H2OContext();
        private H2OAuthentication auth = new H2OAuthentication();
        public ActionResult Index()
        {
            List<Event> events = db.Event.OrderByDescending(x => x.UpdatedDate).ToList();

            HttpCookie myCookie = Request.Cookies["myUserCookie"];
            if (myCookie != null)
            {
                if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
                {
                    string name = myCookie.Values["UserName"].ToString();
                    string email = myCookie.Values["EmailAddress"].ToString();
                    SetUserSession(name, email);
                }
            }
            return View(events);
        }

        public ActionResult Login()
        {
            ViewBag.message = "Login to Hands2Offer";
            return View();
        }

        public ActionResult Logout()
        {
            KillUserSession();
            KillUserCookie();
            return RedirectToAction("index");
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
            ViewBag.message = "Login to Hands2Offer";
            return View();
        }

       // public ActionResult CreateEvent()
       // {
       //     return View();
       // }

        public ActionResult CreateEvent(int? eventId, bool? clone)
        {
            if (eventId ==null || eventId == 0)
            {
                return View();
            }
            else if (clone.HasValue)
            {
                Event evenT = db.Event.Where(x => x.EventId == eventId).FirstOrDefault();
                evenT.EventId = 0;
                return View(evenT);
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
            if (model.EventId == 0)
            {
                db.Event.Add(model);
                db.SaveChanges();
            }
            else
            {
                Event evenT = db.Event.Where(x => x.EventId == model.EventId).FirstOrDefault();
                evenT.ProjectName = model.ProjectName;
                evenT.ProjectDesc = model.ProjectDesc;
                evenT.Address = model.Address;
                evenT.Phone = model.Phone;
                evenT.Email = model.Email;
                evenT.StartDate = model.StartDate;
                evenT.EndDate = model.EndDate;
                evenT.IsActive = model.IsActive;
                evenT.HasCompleted = model.HasCompleted;
                db.SaveChanges();
            }
            return RedirectToAction("Event",new {message = model.ProjectName + " - Event has been added Successfully" });
        }

        public ActionResult DeleteEvent(int deleteEventId)
        {
            Event evenT = db.Event.Where(x => x.EventId == deleteEventId).FirstOrDefault();
            db.Event.Attach(evenT);
            db.Event.Remove(evenT);
            db.SaveChanges();
            return RedirectToAction("Event", new { message = evenT.ProjectName + " - Event has been deleted Successfully" });
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

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var model = new H2OUsers();
            string btn = collection["submitButton"];
            TryUpdateModel(model, collection);
            var existing = db.H2OUsers.Where(x => x.EmailAddress == model.EmailAddress);
            if (existing.Any() && btn== "Login")
            {
                H2OUsers existinguser = existing.FirstOrDefault();
                if (auth.CheckUserAuth(existinguser.EmailAddress, model.Password))
                {
                    SetPersistence(existinguser.UserName, existinguser.EmailAddress, model.EmailPreference.Value);
                    return RedirectToAction("index");
                }
                else
                {
                    ViewBag.message = "User Name or Password invalid";
                    return View();
                }
            }
            else
            {
                if (existing.Any() && btn == "Sign Up")
                {
                    ViewBag.message = "Email already exists";
                    return View();
                }
                model.DateUpdated = DateTime.UtcNow;
                model.Role = 0;
                db.H2OUsers.Add(model);
                db.SaveChanges();
                SetPersistence(model.UserName, model.EmailAddress, model.EmailPreference.Value);
            }            
            return RedirectToAction("index");
        }

        public ActionResult Profiles(string email)
        {
            H2OUsers user = db.H2OUsers.Where(x => x.EmailAddress == email).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult Profiles(FormCollection collection)
        {
            var model = new H2OUsers();
            TryUpdateModel(model, collection);
            H2OUsers user = db.H2OUsers.Where(x => x.EmailAddress == model.EmailAddress).FirstOrDefault();
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.EmailPreference = model.EmailPreference;
            user.DateUpdated = DateTime.UtcNow;
            db.SaveChanges();
            return View(user);
        }

        public ActionResult JoinUs(EventUsers events)
        {
            bool added = false;
            db.EventUsers.Attach(events);
            db.EventUsers.Add(events);
            return Json(added, JsonRequestBehavior.AllowGet);
        }

        private void SetPersistence(string name, string email, bool privateComputer)
        {
            if (privateComputer)
            {
                SetUserCookie(name, email);
            }
            else
            {
                SetUserSession(name, email);
            }
        }

        private void SetUserSession(string name, string email)
        {
            Session["UserName"] = name;
            Session["EmailAddress"] = email;
        }

        private void KillUserSession()
        {
            Session["UserName"] = null;
            Session["EmailAddress"] = null;
        }

        private void SetUserCookie(string name, string email)
        {
            //create a cookie
            HttpCookie myCookie = new HttpCookie("myUserCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("UserName", name);
            myCookie.Values.Add("EmailAddress", email);

            //set cookie expiry date-time. Made it to last for next 12 hours.
            myCookie.Expires = DateTime.Now.AddYears(1);

            //Most important, write the cookie to client.
            Response.Cookies.Add(myCookie);
        }

        private void KillUserCookie()
        {
            HttpCookie myCookie = new HttpCookie("myUserCookie");
            myCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(myCookie);
        }
    }
}