using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class Document
    {           
        [Key]
        public int DocumentId { get; set; }
        [StringLength(500)]
        public string DocumentDescription { get; set; }

        [StringLength(500)]
        public string DocumentLocation { set; get; }
        public int DocumentTypeId { get; set; }
        public int DocumentStatus { get; set; }
        public DateTime UploadDate { get; set; }

        public ApplicationUser User { get; set; }
    }
}