using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace HandsToOfferApi.Models
{
    [Table("[dbo].[Project]")]
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDesc { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}