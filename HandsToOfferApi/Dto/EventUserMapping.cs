using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HandsToOfferApi.Models;


namespace HandsToOfferApi.Dto
{

    public class EventUserMapping {

        public int EventId { get; set; }

        [DisplayName("Event Name")]
        public string ProjectName { get; set; }

        [DisplayName("Description")]
        public string ProjectDesc { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [DisplayName("End Date")]
        public DateTime EndDate { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        [DisplayName("Active")]
        public bool IsActive { get; set; }

        [DisplayName("Completed")]
        public bool HasCompleted { get; set; }

        public string Joining { get; set; }

        public string ImageLocation { get; set; }
    }



    public class LoggedInuser {

        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
    }
}


