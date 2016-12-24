using CareerVisa.Models.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Feature> Features { get; set; }
        public DbSet<Functionality> Functionalities { get; set; }
        public DbSet<CareerField> CareerFields { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentStatus> DocumentStatus { get; set; }
        public DbSet<DocumentsType> DocumentsTypes { get; set; }
        public DbSet<JobSeekerCareerField> JobSeekerCareerFields { get; set; }

    }
}