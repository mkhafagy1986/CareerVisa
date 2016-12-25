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
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());

                ViewBag.JobSeekerCVs = GetDocumentViewModel(context, user.Documents);

                return View(user);
            }
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

        public ActionResult JobSeekerCVs()
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser currentUser = new ApplicationUser();

                currentUser = manager.FindById(User.Identity.GetUserId());
                List<DocumentViewModel> JobSeekerCVs = GetDocumentViewModel(context, currentUser.Documents);
                return View(JobSeekerCVs);
            }

        }

        private static List<DocumentViewModel> GetDocumentViewModel(ApplicationDbContext context, ICollection<Document> UserDocs)
        {
            List<DocumentViewModel> JobSeekerCVs = new List<DocumentViewModel>();
            DocumentViewModel NewDoc;
            foreach (var item in UserDocs)
            {
                if (item.DocumentTypeId != (int)DocType.CurriculumVitae)
                    continue;
                NewDoc = new DocumentViewModel();
                NewDoc.DocumentId = item.DocumentId;
                NewDoc.DocumentDescription = item.DocumentDescription;
                NewDoc.DocumentPath = item.DocumentLocation;
                NewDoc.DocumentStatus = context.DocumentStatus.Where(DS => DS.DocumentStatusId == item.DocumentStatus).ToList().First().DocumentStatusDescription;
                NewDoc.DocumentType = context.DocumentsTypes.Where(DT => DT.DocumentTypeId == item.DocumentTypeId).ToList().First().DocumentTypeDescription;

                JobSeekerCVs.Add(NewDoc);
            }

            return JobSeekerCVs;
        }

        [Authorize(Roles = "JobSeeker")]
        public PartialViewResult AddJobSeekerCV()
        {
            return PartialView("_AddJobSeekerCV", new DocumentViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public ActionResult AddJobSeekerCV(DocumentViewModel model, HttpPostedFileBase UploadCurriculumVitae)
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());

                if (UploadCurriculumVitae == null)
                    return View("JobSeekerCVs", user.CareerFields);

                if (ModelState.IsValid)
                {
                    var extintion = "";
                    var filename = "";
                    var userfolderpath = Server.MapPath("~/JobSeekersCurriculumVitae/" + user.Id);
                    if (UploadCurriculumVitae != null)
                    {
                        extintion = Path.GetExtension(UploadCurriculumVitae.FileName);
                        filename = Path.GetFileName(UploadCurriculumVitae.FileName);
                        if (!new DirectoryInfo(userfolderpath).Exists)
                            new DirectoryInfo(userfolderpath).Create();
                        else
                        {
                            string[] files = System.IO.Directory.GetFiles(userfolderpath, string.Format("{0}", filename));
                            if (files.Length > 0)
                            {
                                System.IO.File.Delete(files[0]);
                            }
                        }

                        UploadCurriculumVitae.SaveAs(string.Format("{0}/{1}", userfolderpath, filename));

                    }

                    Document NewCurriculumVitae = new Document
                    {
                        DocumentDescription = model.DocumentDescription,
                        DocumentLocation = string.Format("/JobSeekersCurriculumVitae/" + "{0}/{1}", user.Id, filename),
                        DocumentStatus = (int)DocStatus.Pending,
                        DocumentTypeId = (int)DocType.CurriculumVitae
                    };

                    user.Documents.Add(NewCurriculumVitae);
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());

                    return View("JobSeekerCVs", GetDocumentViewModel(context, user.Documents));
                }
                return View("Index", user);
            }
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult DeleteJobSeekerCV(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {

                    user.Documents.Remove(user.Documents.Where(cv => cv.DocumentId == Id).ToList().First());
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());
                    return View("JobSeekerCVs", GetDocumentViewModel(context, user.Documents));
                }
                return View("Index", user);
            }
        }

        [Authorize(Roles = "JobSeeker")]
        public FileResult DownloadFile(string FilePath)
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(FilePath));
            string fileName = new FileInfo(Server.MapPath(FilePath)).Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}