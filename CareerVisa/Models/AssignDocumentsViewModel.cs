using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class NotAssignDocumentsViewModel
    {
        public string DocumentOwnerUsername { get; set; }
        public string DocumentOwnerUserId { get; set; }
        public int DocumentId { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentStatus { get; set; }
        public int DocumentStatusId { get; set; }
        public string DocumentType { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime DocumentUploadDate { get; set; }
        public string ReviewerUserId { get; set; }
        public string ReviewerUserName { get; set; }
        public DateTime AssignDate { get; set; }
    }
}