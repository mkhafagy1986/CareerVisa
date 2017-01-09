using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class AssignedDocument
    {
        [Key]
        public int AssignedDocumentId { get; set; }
        public string OwnerUserId { get; set; }
        public DateTime AssignedDate { get; set; }
        public string AdministratorUserId { get; set; }
        public int DocumentId { get; set; }
        public string ReviewerUserId { get; set; }
    }
}