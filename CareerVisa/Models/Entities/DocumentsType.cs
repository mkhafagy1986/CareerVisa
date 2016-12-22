using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class DocumentsType
    {
        [Key]
        public int DocumentTypeId { get; set; }
        public string DocumentTypeDescription { get; set; }
    }
}