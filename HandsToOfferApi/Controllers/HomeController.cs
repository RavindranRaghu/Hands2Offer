using System;
using System.Collections.Generic;
using System.Drawing;
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
            int userIdNumber = 0;
            string userid = string.Empty;
            string name = string.Empty;
            string email = string.Empty;
            HttpCookie myCookie = Request.Cookies["myUserCookie"];
            if (myCookie != null)
            {
                if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
                {
                    userid = (myCookie.Values["UserId"] != null) ? myCookie.Values["UserId"].ToString() : "";
                    name = myCookie.Values["UserName"].ToString();
                    email = myCookie.Values["EmailAddress"].ToString();
                    SetUserSession(userid, name, email);
                }
            }

            bool isUseridValid = Int32.TryParse(userid, out userIdNumber);
            if (!isUseridValid)
                userIdNumber = 0;

            List<EventUserMapping> eventList = (from events in db.Event
                                                join map in db.EventUsers on
                                                new { Key1 = events.EventId, Key2 = userIdNumber } equals new { Key1 = map.EventId, Key2 = map.UserId }
                                                into mapAll
                                                from map in mapAll.DefaultIfEmpty()
                                                select new EventUserMapping {
                                                    EventId = events.EventId,
                                                    ProjectName = events.ProjectName,
                                                    ProjectDesc = events.ProjectDesc,
                                                    StartDate = events.StartDate,
                                                    EndDate = events.EndDate,
                                                    Address = events.Address,
                                                    Email = events.Email,
                                                    Phone = events.Phone,
                                                    HasCompleted = events.HasCompleted,
                                                    IsActive = events.IsActive,
                                                    Joining = map.Joining == null ? "Havenotdecided" : map.Joining,
                                                    ImageLocation = db.ImageUpload.Where(x => x.EventId == events.EventId).Select(y=>y.ImageLocation).FirstOrDefault()
                                                 }).ToList();

            return View(eventList);
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
                    SetPersistence(existinguser.Id.ToString(), existinguser.UserName, existinguser.EmailAddress, model.EmailPreference.Value);
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
                SetPersistence(model.Id.ToString(), model.UserName, model.EmailAddress, model.EmailPreference.Value);
            }            
            return RedirectToAction("index");
        }

        public ActionResult Profiles(string userid)
        {
            int UserId = 0;
            bool IsvalidId = Int32.TryParse(userid, out UserId);

            HttpCookie myCookie = Request.Cookies["myUserCookie"];
            bool isCookieExist = (string.IsNullOrEmpty(myCookie.Values["UserName"])) ? false : true;
            if (IsvalidId)
            {
                H2OUsers user = db.H2OUsers.Where(x => x.Id == UserId).FirstOrDefault();
                SetPersistence(user.Id.ToString(), user.UserName, user.EmailAddress, isCookieExist);
                ViewBag.Message = "User Profile";
                return View(user);
            }
            else {
                return View();
            }

        }

        [HttpPost]
        public ActionResult Profiles(FormCollection collection)
        {
            var model = new H2OUsers();
            TryUpdateModel(model, collection);
            H2OUsers user = db.H2OUsers.Where(x => x.Id == model.Id).FirstOrDefault();
            user.EmailAddress = model.EmailAddress;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;
            user.EmailPreference = model.EmailPreference;
            user.DateUpdated = DateTime.UtcNow;
            db.SaveChanges();
            HttpCookie myCookie = Request.Cookies["myUserCookie"];
            bool isCookieExist = (string.IsNullOrEmpty(myCookie.Values["UserName"])) ? false : true;
            SetPersistence(user.Id.ToString(), user.UserName, user.EmailAddress, isCookieExist);
            ViewBag.Message = "User Profile saved Successfully";
            return View(user);
        }

        public ActionResult JoinUs(EventUsers eventUser)
        {
            bool added = false;
            try
            {
                var eventUserExist = db.EventUsers.Where(x => x.UserId == eventUser.UserId && x.EventId == eventUser.EventId);
                if (eventUserExist.Any())
                {
                    EventUsers newEventUser = eventUserExist.First();
                    newEventUser.EmailAddress = eventUser.EmailAddress;
                    newEventUser.PhoneNumber = eventUser.PhoneNumber;
                    newEventUser.Joining = eventUser.Joining;
                    newEventUser.UpdatedDate = DateTime.UtcNow;
                }
                else
                {
                    eventUser.UpdatedDate = DateTime.UtcNow;
                    db.EventUsers.Attach(eventUser);
                    db.EventUsers.Add(eventUser);
                }
                db.SaveChanges();
                added = true;
                return Json(added, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(added, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ImageUploader(int? eventId) {
            List<ImageUpload> imageList = db.ImageUpload.Where(x => x.EventId == eventId).ToList();
            ViewBag.EventId = (eventId== null)?0 : eventId;
            return View(imageList);
        }

        [HttpPost]
        public ActionResult ImageUploaderPost(ImageUpload img, HttpPostedFileBase file, int RecentEventId)
        {
            if (file != null)
            {
                file.SaveAs(HttpContext.Server.MapPath("~/Content/recent/")
                                                      + file.FileName);
                img.EventId = RecentEventId;
                img.ImageLocation = file.FileName;
            }
            db.ImageUpload.Attach(img);
            db.ImageUpload.Add(img);
            db.SaveChanges();
            return RedirectToAction("ImageUploader", new { eventId = RecentEventId });
        }

        public ActionResult ImageDelete(int imageId)
        {
            ImageUpload image = db.ImageUpload.Where(x => x.ImageId == imageId).FirstOrDefault();
            var filePath = Server.MapPath("~/Content/recent/" + image.ImageLocation);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            db.ImageUpload.Remove(image);
            db.SaveChanges();
            return RedirectToAction("ImageUploader", new { eventId = image.EventId });
        }

        public ActionResult EventDetails(int eventId)
        {
            EventDetailsDto details = new EventDetailsDto(eventId);
           return View(details);
        }

        private void SetPersistence(string userid, string name, string email, bool privateComputer)
        {
            if (privateComputer)
            {
                SetUserCookie(userid, name, email);
            }
            else
            {
                SetUserSession(userid, name, email);
            }
        }

        private void SetUserSession(string userid, string name, string email)
        {
            Session["UserId"] = userid.ToString();
            Session["UserName"] = name;
            Session["EmailAddress"] = email;
        }

        private void KillUserSession()
        {
            Session["UserId"] = null;
            Session["UserName"] = null;
            Session["EmailAddress"] = null;
        }

        private void SetUserCookie(string userid, string name, string email)
        {
            //create a cookie
            HttpCookie myCookie = new HttpCookie("myUserCookie");

            //Add key-values in the cookie
            myCookie.Values.Add("UserId", userid.ToString());
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