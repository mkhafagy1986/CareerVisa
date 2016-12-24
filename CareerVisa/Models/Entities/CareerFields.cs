using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class CareerField
    {
        [Key]
        public int CareerFieldId { get; set; }

        [StringLength(500)]
        [Display(Name="Career Field:")]
        public string CareerFieldName { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}