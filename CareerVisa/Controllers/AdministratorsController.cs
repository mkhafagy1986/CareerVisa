

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
        private ApplicationUserManager _userManager;

        public AdministratorsController()
        {
        }

        public AdministratorsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
        }



        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private RoleManager<IdentityRole>  _RoleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                return _RoleManager ?? new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            }
            set { _RoleManager = value; }
        }

        // GET: Administrators
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            ViewBag.NotAssignedCoverLettersCount = 100;
            ViewBag.NotAssignedCVsCount = 200;
            ViewBag.EmployersCount = 150;
            ViewBag.JobSeekerCount = 250;

            RoleManager.
            return View();
        }
    }
}