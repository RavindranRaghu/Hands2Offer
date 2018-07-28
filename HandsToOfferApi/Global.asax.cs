using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace HandsToOfferApi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            HttpCookie myCookie = Request.Cookies ["myUserCookie"];
            if (myCookie != null)
            {
                if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
                {
                    string userid = (myCookie.Values["UserId"] != null) ? myCookie.Values["UserId"].ToString() : "";
                    string name = myCookie.Values["UserName"].ToString();
                    string email = myCookie.Values["EmailAddress"].ToString();
                    Session["UserId"] = userid.ToString();
                    Session["UserName"] = name;
                    Session["EmailAddress"] = email;
                }
            }
        }
    }
}
