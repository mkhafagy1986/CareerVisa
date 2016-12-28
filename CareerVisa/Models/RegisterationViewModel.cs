using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class RegisterationViewModel
    {
        public EmployerModel Employer { get; set; }
        public JobSeekerModel JobSeeker { get; set; }
        [Required]
        [Display(Name="Registration Type")]
        public int RegistrationType { get; set; }
    }
}