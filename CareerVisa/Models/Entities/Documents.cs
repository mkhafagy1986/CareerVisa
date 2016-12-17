using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class Documents
    {           
        public int DocumentId { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentLocation { set; get; }
        public int DocumentTypeId { get; set; }
        public int DocumentStatus { get; set; }
    }
}