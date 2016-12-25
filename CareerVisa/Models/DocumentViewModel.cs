using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class DocumentViewModel
    {
        public int DocumentId { get; set; }
        public string DocumentDescription { get; set; }
        public string DocumentStatus { get; set; }
        public string DocumentType { get; set; }
        public string DocumentPath { get; set; }
    }

    public enum DocStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }

    public enum DocType
    {
        CurriculumVitae = 1,
        CoverLetters = 2,
        EvaluationReport = 3
    }
}