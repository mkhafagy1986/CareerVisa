using CareerVisa.Models;
using CareerVisa.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CareerVisa.Controllers
{
    public class JobSeekerController : Controller
    {
        // GET: JobSeeker
        [Authorize(Roles = "JobSeeker")]
        public ActionResult Index()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());

            return View(currentUser);
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult EditProfile()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());

            return View(currentUser);
        }

        [Authorize(Roles = "JobSeeker")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditProfile(ApplicationUser UserProfile, HttpPostedFileBase UploadProfilePicture)
        {

            if (ModelState.IsValid)
            {
                var extintion = "";
                if (UploadProfilePicture != null)
                {
                    extintion = Path.GetExtension(UploadProfilePicture.FileName);
                    var userfolderpath = Server.MapPath("~/UserProfilePictures/" + UserProfile.Id);
                    if (!new DirectoryInfo(userfolderpath).Exists)
                        new DirectoryInfo(userfolderpath).Create();
                    else
                    {
                        string[] files = System.IO.Directory.GetFiles(userfolderpath, "profilepic.*");
                        if (files.Length > 0)
                        {
                            System.IO.File.Delete(files[0]);
                        }
                    }

                    UploadProfilePicture.SaveAs(string.Format("{0}/profilepic{1}", userfolderpath, extintion));

                }


                using (var context = new ApplicationDbContext())
                {
                    var store = new UserStore<ApplicationUser>(context);
                    var manager = new UserManager<ApplicationUser>(store);
                    ApplicationUser user = new ApplicationUser();

                    user = manager.FindById(UserProfile.Id);
                    user.FirstName = UserProfile.FirstName;
                    user.Lastname = UserProfile.Lastname;
                    user.PhoneNumber = UserProfile.PhoneNumber;
                    user.Address = UserProfile.Address;
                    user.LinkedInURL = UserProfile.LinkedInURL;
                    user.WebsiteURL = UserProfile.WebsiteURL;
                    if (UploadProfilePicture != null)
                        user.PersonalPhotoPath = string.Format("{0}/profilepic{1}", UserProfile.Id, extintion);

                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());
                    return RedirectToAction("Index", "JobSeeker");
                }
            }
            return View(UserProfile);
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult CareerFields()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());

            return View(currentUser.CareerFields);
        }

        [Authorize(Roles = "JobSeeker")]
        public PartialViewResult AddCareerField()
        {
            return PartialView("_AddCareerField", new CareerField());
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public ActionResult AddCareerField(CareerField model)
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {

                    user.CareerFields.Add(context.CareerFields.Where(cf => cf.CareerFieldId == model.CareerFieldId).ToList().First());
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());
                    return View("CareerFields", user.CareerFields);
                }
            return View("Index", user);
            }
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult DeleteCareerField(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {

                    user.CareerFields.Remove(user.CareerFields.Where(cf => cf.CareerFieldId == Id).ToList().First());
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());
                    return View("CareerFields", user.CareerFields);
                }
                return View("Index", user);
            }
        }
    }
}