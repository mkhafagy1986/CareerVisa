using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        [Display(Name = "Document Description")]
        [Required]
        public string DocumentDescription { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentType { get; set; }
        [Display(Name = "Document Location")]
        public string DocumentPath { get; set; }
    }

    public enum DocStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Assigned = 4
    }

    public enum DocType
    {
        CurriculumVitae = 1,
        CoverLetters = 2,
        EvaluationReport = 3
    }
}