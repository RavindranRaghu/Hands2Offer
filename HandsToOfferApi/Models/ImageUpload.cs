using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HandsToOfferApi.Models
{
    [Table("[dbo].[ImageUpload]")]
    public class ImageUpload
    {
        [Key]
        public int ImageId { get; set; }
        public int EventId { get; set; }
        public string ImageLocation { get; set; }
    }
}