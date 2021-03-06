﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using CareerVisa.Models.Entities;
using System.Collections.Generic;

namespace CareerVisa.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string Lastname { get; set; }

        [Required]
        [StringLength(14)]
        [Display(Name = "Phone Number")]
        public override string PhoneNumber { get; set; }

        public string Address { get; set; }
        [Display(Name = "LinkedIn URL")]
        [Url]
        public string LinkedInURL { get; set; }
        public string PersonalPhotoPath { get; set; }
        [Display(Name = "Website URL")]
        [Url]
        public string WebsiteURL { get; set; }

        public virtual ICollection<CareerField> CareerFields { get; set; }
        public virtual ICollection<Document> Documents { get; set; }

    }


}