using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandsToOfferApi.Models
{
    [Table("[dbo].[EventUsers]")]
    public class EventUsers
    {
        [Key]
        public int ProjectUserId { get; set; }

        public int EventId { get; set; }

        public int UserId { get; set; }

        public string Joining { get; set; }

        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }

        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }

        public DateTime UpdatedDate { get; set; }

    }
}