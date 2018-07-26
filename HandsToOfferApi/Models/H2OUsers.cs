using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandsToOfferApi.Models
{
    [Table("[dbo].[H2OUsers]")]
    public class H2OUsers
    {
        public int Id { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public int? Role { get; set; }

        [DisplayName("Email Preference")]
        public bool? EmailPreference { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}