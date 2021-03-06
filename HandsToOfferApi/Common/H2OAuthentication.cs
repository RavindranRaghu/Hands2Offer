﻿using HandsToOfferApi.Models;
using HandsToOfferApi.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HandsToOfferApi.Common
{
    //public class AuthorizeAdminOrOwnerOfPostAttribute : AuthorizeAttribute
    //{
    //    protected override bool AuthorizeCore(HttpContextBase httpContext)
    //    {
    //        var authorized = base.AuthorizeCore(httpContext);
    //        if (!authorized)
    //        {
    //            // The user is not authenticated
    //            return false;
    //        }

    //        var user = httpContext.User;
    //        if (user.IsInRole("Admin"))
    //        {
    //            // Administrator => let him in
    //            return true;
    //        }

    //        var rd = httpContext.Request.RequestContext.RouteData;
    //        var id = rd.Values["id"] as string;
    //        if (string.IsNullOrEmpty(id))
    //        {
    //            // No id was specified => we do not allow access
    //            return false;
    //        }

    //        return IsOwnerOfPost(user.Identity.Name, id);
    //    }

    //    private bool IsOwnerOfPost(string username, string postId)
    //    {
    //        // TODO: you know what to do here
    //        throw new NotImplementedException();
    //    }
    //}


    public class H2OAuthentication
    {
        private H2OContext db = new H2OContext();
        public bool CheckUserAuth(string email, string pd)
        {
            bool authorized = false;
            if (db.H2OUsers.Where(x => x.EmailAddress == email && x.Password == pd).Any())
            {
                authorized = true;
            }
            return authorized;
        }
    }

    public class LoginProcess
    {
        private HttpContextBase Context { get; set; }

        public LoginProcess(HttpContextBase context)
        {
            this.Context = context;
        }

        public void Bar()
        {
            HttpCookie myCookie = Context.Request.Cookies["myUserCookie"];
            if (myCookie != null)
            {
                if (!string.IsNullOrEmpty(myCookie.Values["UserName"]))
                {
                    string userid = (myCookie.Values["UserId"] != null) ? myCookie.Values["UserId"].ToString() : "";
                    string name = myCookie.Values["UserName"].ToString();
                    string email = myCookie.Values["EmailAddress"].ToString();
                    //SetUserSession(userid, name, email);
                }
            }
        }
    }
}