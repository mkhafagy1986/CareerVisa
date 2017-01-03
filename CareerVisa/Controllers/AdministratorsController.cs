

using CareerVisa.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerVisa.Controllers
{
    public class AdministratorsController : Controller
    {
        private UsersCountViewModel UsersCount;

        public AdministratorsController()
        {
            UsersCount = new UsersCountViewModel();
        }

        public AdministratorsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UsersCount = new UsersCountViewModel();
        }

        // GET: Administrators
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            GetEmployersCount();
            ViewBag.NotAssignedCoverLettersCount = UsersCount.NotAssignedCoverLettersCount;
            ViewBag.NotAssignedCVsCount = UsersCount.NotAssignedCVsCount;
            ViewBag.EmployersCount = UsersCount.EmployersCount;
            ViewBag.JobSeekerCount = UsersCount.JobSeekerCount;

            return View();
        }

        private void GetEmployersCount()
        {
            using (var context = new ApplicationDbContext())
            {

                UsersCount.EmployersCount = context.Users.Where(user => user.Roles.Any(role => role.RoleId == "4")).Count();
                UsersCount.JobSeekerCount = context.Users.Where(user => user.Roles.Any(role => role.RoleId == "3")).Count();


            }

            //foreach (var user in UserManager.Users.ToList())
            //{
            //    if (UserManager.IsInRole(user.Id, "Employer"))
            //        UsersCount.EmployersCount++;
            //    else if (UserManager.IsInRole(user.Id, "JobSeeker"))
            //        UsersCount.JobSeekerCount++;
            //}

        }
    }
}