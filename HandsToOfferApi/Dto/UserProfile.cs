using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HandsToOfferApi.Models;


namespace HandsToOfferApi.Dto
{
    public class UserProfile
    {
        public string EmailAddress { get; set;}
        public string UserName { get; set; }
        public List<Event> EventList { get; set; }
    }
}


