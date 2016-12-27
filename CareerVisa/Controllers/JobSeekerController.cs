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
                ViewBag.CareerFieldsCount = user.CareerFields.Count;
                ViewBag.JobSeekerCVs = GetDocumentViewModel(context, user.Documents, DocType.CurriculumVitae);
                ViewBag.JobSeekerCoverLetters = GetDocumentViewModel(context, user.Documents, DocType.CoverLetters);
                SetUserFullName(user);

                return View(user);
            }
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult EditProfile()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

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
                    SetUserFullName(user);

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
            SetUserFullName(currentUser);

            return View(currentUser.CareerFields);
        }

        [Authorize(Roles = "JobSeeker")]
        public PartialViewResult AddCareerField()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

            return PartialView("_AddCareerField", new CareerField());
        }

        private void SetUserFullName(ApplicationUser currentUser)
        {
            ViewBag.UserFullName = string.Format("{0} {1}", currentUser.FirstName, currentUser.Lastname);
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
                SetUserFullName(user);

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
                    ViewBag.UserFullName = string.Format("{0} {1}", user.FirstName, user.Lastname);

                    return View("CareerFields", user.CareerFields);
                }
                SetUserFullName(user);

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
                List<DocumentViewModel> JobSeekerCVs = GetDocumentViewModel(context, currentUser.Documents, DocType.CurriculumVitae);
                SetUserFullName(currentUser);

                return View(JobSeekerCVs);
            }

        }

        private static List<DocumentViewModel> GetDocumentViewModel(ApplicationDbContext context, ICollection<Document> UserDocs, DocType DocumentType)
        {
            List<DocumentViewModel> JobSeekerCVs = new List<DocumentViewModel>();
            DocumentViewModel NewDoc;
            foreach (var item in UserDocs)
            {
                if (item.DocumentTypeId != (int)DocumentType)
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
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

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
                    return View("JobSeekerCVs", GetDocumentViewModel(context, user.Documents, DocType.CurriculumVitae));

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
                    SetUserFullName(user);

                    return View("JobSeekerCVs", GetDocumentViewModel(context, user.Documents, DocType.CurriculumVitae));
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
                    var DeletedDocument = user.Documents.Where(cv => cv.DocumentId == Id).ToList().First();
                    string DocumentPath = DeletedDocument.DocumentLocation;

                    context.Documents.Remove(user.Documents.Where(cv => cv.DocumentId == Id).ToList().First());
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());

                    var DocPhysicalPath = Server.MapPath("~" + DocumentPath);
                    FileInfo file = new FileInfo(DocPhysicalPath);
                    file.Delete();
                    SetUserFullName(user);

                    return View("JobSeekerCVs", GetDocumentViewModel(context, user.Documents, DocType.CurriculumVitae));
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


        ////////////////////////////////////Cover letters ///////////////////////////////
        public ActionResult JobSeekerCoverLetters()
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser currentUser = new ApplicationUser();

                currentUser = manager.FindById(User.Identity.GetUserId());
                List<DocumentViewModel> JobSeekerCoverLetters = GetDocumentViewModel(context, currentUser.Documents, DocType.CoverLetters);
                SetUserFullName(currentUser);

                return View(JobSeekerCoverLetters);
            }

        }

        [Authorize(Roles = "JobSeeker")]
        public PartialViewResult AddJobSeekerCoverLetter()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

            return PartialView("_AddJobSeekerCoverLetter", new DocumentViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "JobSeeker")]
        public ActionResult AddJobSeekerCoverLetter(DocumentViewModel model, HttpPostedFileBase UploadCoverLetter)
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());

                if (UploadCoverLetter == null)
                    return View("JobSeekerCoverLetters", GetDocumentViewModel(context, user.Documents, DocType.CoverLetters));

                if (ModelState.IsValid)
                {
                    var extintion = "";
                    var filename = "";
                    var userfolderpath = Server.MapPath("~/JobSeekersCoverLetter/" + user.Id);
                    if (UploadCoverLetter != null)
                    {
                        extintion = Path.GetExtension(UploadCoverLetter.FileName);
                        filename = Path.GetFileName(UploadCoverLetter.FileName);
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

                        UploadCoverLetter.SaveAs(string.Format("{0}/{1}", userfolderpath, filename));

                    }

                    Document NewCoverLetter = new Document
                    {
                        DocumentDescription = model.DocumentDescription,
                        DocumentLocation = string.Format("/JobSeekersCoverLetter/" + "{0}/{1}", user.Id, filename),
                        DocumentStatus = (int)DocStatus.Pending,
                        DocumentTypeId = (int)DocType.CoverLetters
                    };

                    user.Documents.Add(NewCoverLetter);
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());
                    SetUserFullName(user);

                    return View("JobSeekerCoverLetters", GetDocumentViewModel(context, user.Documents, DocType.CoverLetters));
                }
                SetUserFullName(user);

                return View("Index", user);
            }
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult DeleteJobSeekerCoverLetter(int Id)
        {
            using (var context = new ApplicationDbContext())
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                ApplicationUser user = new ApplicationUser();

                user = manager.FindById(User.Identity.GetUserId());
                if (ModelState.IsValid)
                {
                    var DeletedDocument = user.Documents.Where(cv => cv.DocumentId == Id).ToList().First();
                    string DocumentPath = DeletedDocument.DocumentLocation;

                    context.Documents.Remove(DeletedDocument);
                    Task.WaitAny(manager.UpdateAsync(user));

                    Task.WaitAny(context.SaveChangesAsync());

                    var DocPhysicalPath = Server.MapPath("~" + DocumentPath);
                    FileInfo file = new FileInfo(DocPhysicalPath);
                    file.Delete();
                    SetUserFullName(user);

                    return View("JobSeekerCoverLetters", GetDocumentViewModel(context, user.Documents, DocType.CoverLetters));
                }
                SetUserFullName(user);

                return View("Index", user);
            }
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult CVBuilder()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

            return View();
        }

        [Authorize(Roles = "JobSeeker")]
        public ActionResult ReviewReports()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

            return View();
        }

        [Authorize(Roles = "JobSeeker")]
        public ViewResult ViewProfile()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            // Get the current logged in User and look up the user in ASP.NET Identity
            var currentUser = manager.FindById(User.Identity.GetUserId());
            SetUserFullName(currentUser);

            return View(currentUser);
        }
    }
}