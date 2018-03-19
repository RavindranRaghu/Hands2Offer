using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel;

namespace HandsToOfferApi.Models
{
    [Table("[dbo].[Event]")]
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        public int ProjectId { get; set; }

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

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
    }
}