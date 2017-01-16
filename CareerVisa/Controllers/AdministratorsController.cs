

using CareerVisa.App_Start;
using CareerVisa.Models;
using CareerVisa.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CareerVisa.Controllers
{
    public class AdministratorsController : Controller
    {
        private UsersCountViewModel UsersCount;
        IEnumerable<NotAssignDocumentsViewModel> NotAssignDocuments;
        EncryptionHelper EncryptionHelper;
        public AdministratorsController()
        {
            UsersCount = new UsersCountViewModel();
            EncryptionHelper = new EncryptionHelper(ConfigurationManager.AppSettings["EncryptionKey"]);
        }

        public AdministratorsController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UsersCount = new UsersCountViewModel();
            EncryptionHelper = new EncryptionHelper(ConfigurationManager.AppSettings["EncryptionKey"]);
        }

        // GET: Administrators
        [Authorize(Roles = "Administrators")]
        public ActionResult Index()
        {
            GetAdminData();
            return View();
        }

        private void GetUsersCount()
        {
            using (var context = new ApplicationDbContext())
            {
                UsersCount.EmployersCount = context.Users.Where(user => user.Roles.Any(role => role.RoleId == "4")).Count();
                UsersCount.JobSeekerCount = context.Users.Where(user => user.Roles.Any(role => role.RoleId == "3")).Count();
            }
        }

        private void GetNotAssignDocumentsCount()
        {
            using (var context = new ApplicationDbContext())
            {
                UsersCount.NotAssignedCVsCount = context.Documents.Where(doc => doc.DocumentTypeId == (int)DocType.CurriculumVitae && doc.DocumentStatus == (int)DocStatus.Pending).Count();
                UsersCount.NotAssignedCoverLettersCount = context.Documents.Where(doc => doc.DocumentTypeId == (int)DocType.CoverLetters && doc.DocumentStatus == (int)DocStatus.Pending).Count();
            }
        }

        public void GetAdminData()
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());

            GetUsersCount();
            GetNotAssignDocumentsCount();
            ViewBag.NotAssignedCoverLettersCount = UsersCount.NotAssignedCoverLettersCount;
            ViewBag.NotAssignedCVsCount = UsersCount.NotAssignedCVsCount;
            ViewBag.EmployersCount = UsersCount.EmployersCount;
            ViewBag.JobSeekerCount = UsersCount.JobSeekerCount;
            ViewBag.UserFullName = SetUserFullName(currentUser);
        }

        private string SetUserFullName(ApplicationUser currentUser)
        {
            return string.Format("{0} {1}", currentUser.FirstName, currentUser.Lastname);
        }

        [Authorize(Roles = "Administrators")]
        public ViewResult AssignCurriculumVitae()
        {
            GetAdminData();
            return View(GetNotAssignDocuments().Where(doc => doc.DocumentTypeId == (int)DocType.CurriculumVitae && doc.DocumentStatusId == (int)DocStatus.Pending));
        }

        public IEnumerable<NotAssignDocumentsViewModel> GetNotAssignDocuments()
        {
            IEnumerable<NotAssignDocumentsViewModel> NotAssignDocuments = new List<NotAssignDocumentsViewModel>();
            using (var context = new ApplicationDbContext())
            {
                NotAssignDocuments = context.Documents.Select(
                NotAssignDocument => new NotAssignDocumentsViewModel
                {
                    DocumentId = NotAssignDocument.DocumentId.ToString(),
                    DocumentDescription = NotAssignDocument.DocumentDescription,
                    AssignDate = DateTime.Now,
                    DocumentOwnerUserId = NotAssignDocument.User.Id,
                    DocumentOwnerUsername = NotAssignDocument.User.UserName,
                    DocumentStatusId = NotAssignDocument.DocumentStatus,

                    DocumentStatus = (NotAssignDocument.DocumentStatus == 1 ? DocStatus.Pending.ToString() : (NotAssignDocument.DocumentStatus == 2 ? DocStatus.Approved.ToString() : (NotAssignDocument.DocumentStatus == 3 ? DocStatus.Rejected.ToString() : (NotAssignDocument.DocumentStatus == 4 ? DocStatus.Assigned.ToString() : "")))),
                    DocumentTypeId = NotAssignDocument.DocumentTypeId,
                    DocumentType = (NotAssignDocument.DocumentTypeId == 1 ? DocType.CurriculumVitae.ToString() : (NotAssignDocument.DocumentTypeId == 2 ? DocType.CoverLetters.ToString() : (NotAssignDocument.DocumentTypeId == 3 ? DocType.EvaluationReport.ToString() : ""))),
                    DocumentUploadDate = NotAssignDocument.UploadDate
                })
                .ToList();
            }
            return NotAssignDocuments;
        }

        [Authorize(Roles = "Administrators")]
        public PartialViewResult Assign(string EncryptedDocumentId, string EncryptedOwnerId)
        {
            GetAdminData();
            AssignedDocument DocumentToAssign = new AssignedDocument();

            string DocumentId = EncryptionHelper.Decrypt(EncryptedDocumentId);
            string OwnerId = EncryptionHelper.Decrypt(EncryptedOwnerId);

            DocumentToAssign.OwnerUserId = OwnerId;
            DocumentToAssign.AssignedDate = DateTime.Now;
            DocumentToAssign.AdministratorUserId = User.Identity.GetUserId();
            DocumentToAssign.DocumentId = Int32.Parse(DocumentId);

            return PartialView("_Reviewers", DocumentToAssign);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Assign(AssignedDocument DocumentToAssign)
        {
            using (var context = new ApplicationDbContext())
            {
                context.AssignedDocuments.Add(DocumentToAssign);
                var OrignalDocument = context.Documents.Where(doc => doc.DocumentId == DocumentToAssign.DocumentId).First();
                OrignalDocument.DocumentStatus = (int)DocStatus.Assigned;

                context.SaveChanges();
            }

            return RedirectToAction("AssignCurriculumVitae");
        }

        [Authorize(Roles = "Administrators")]
        public PartialViewResult AssignSelected(FormCollection form)
        {
            GetAdminData();
            AssignedDocument DocumentToAssign = new AssignedDocument();
            var selectedIds = form.GetValues("selectedDocument");

            return PartialView("_Reviewers", DocumentToAssign);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult AssignSelected(FormCollection form, AssignedDocument DocumentToAssign)
        {
            var selectedIds = form.GetValues("selectedDocument");
            if (selectedIds != null)
            {
                List<AssignedDocument> AssignedDocumentlist = new List<AssignedDocument>();
                foreach (var id in selectedIds)
                {
                    var currentAssignedDocument = GetAssignedDocument(id);
                    currentAssignedDocument.ReviewerUserId = DocumentToAssign.ReviewerUserId;
                    AssignedDocumentlist.Add(currentAssignedDocument);
                }
            }

            return RedirectToAction("AssignCurriculumVitae");
        }

        public AssignedDocument GetAssignedDocument(string DocumentId)
        {
            AssignedDocument DocumentToAssign = new AssignedDocument();
            using (var context = new ApplicationDbContext())
            {
                var DocumentObject = context.Documents.Where(doc => doc.DocumentId.ToString() == DocumentId).First();
                if(DocumentObject!=null)
                {
                    DocumentToAssign.OwnerUserId = DocumentObject.User.Id;
                    DocumentToAssign.AssignedDate = DateTime.Now;
                    DocumentToAssign.AdministratorUserId = User.Identity.GetUserId();
                    DocumentToAssign.DocumentId = DocumentObject.DocumentId;
                }
            }
            return DocumentToAssign;
        }
    }
}