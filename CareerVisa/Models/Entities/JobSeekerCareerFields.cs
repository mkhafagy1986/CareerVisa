using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class JobSeekerCareerField
    {
        [Key]
        public int JobSeekerCareerFieldsId { get; set; }
        public string JobSeekerId { get; set; }
        public int CareerFieldId { get; set; }
    }
}