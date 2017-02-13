

using CareerVisa.App_Start;
using CareerVisa.Models;
using CareerVisa.Models.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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

        public IEnumerable<NotAssignDocumentsViewModel> GetNotAssignDocuments(string[] SelectedDocumentIds)
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
                }).Where(NotAssignDocument => SelectedDocumentIds.Contains(NotAssignDocument.DocumentId))
                .ToList();
            }
            return NotAssignDocuments;
        }

        [Authorize(Roles = "Administrators")]
        public PartialViewResult Assign(string EncryptedDocumentId, string EncryptedOwnerId)
        {
            GetAdminData();
            AssignedDocument DocumentToAssign = new AssignedDocument();

            //string DocumentId = EncryptionHelper.Decrypt(EncryptedDocumentId);
            //string OwnerId = EncryptionHelper.Decrypt(EncryptedOwnerId);

            //DocumentToAssign.OwnerUserId = OwnerId;
            DocumentToAssign.OwnerUserId = EncryptedOwnerId;
            DocumentToAssign.AssignedDate = DateTime.Now;
            DocumentToAssign.AdministratorUserId = User.Identity.GetUserId();
            //DocumentToAssign.DocumentId = Int32.Parse(DocumentId);
            DocumentToAssign.DocumentId = Int32.Parse(EncryptedDocumentId);

            return PartialView("_Reviewers", DocumentToAssign);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult Assign(AssignedDocument DocumentToAssign)
        {
            List<AssignedDocument> AssignedDocumentlist = new List<AssignedDocument>();
            AssignedDocumentlist.Add(DocumentToAssign);
            AssignDocuments(AssignedDocumentlist);

            return RedirectToAction("AssignCurriculumVitae");
        }

        private static void AssignDocuments(List<AssignedDocument> AssignedDocumentlist)
        {
            foreach (var AssignedDocument in AssignedDocumentlist)
            {
                using (var context = new ApplicationDbContext())
                {
                    context.AssignedDocuments.Add(AssignedDocument);
                    var OrignalDocument = context.Documents.Where(doc => doc.DocumentId == AssignedDocument.DocumentId).FirstOrDefault();
                    OrignalDocument.DocumentStatus = (int)DocStatus.Assigned;

                    context.SaveChanges();
                }
            }

        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult AssignCurriculumVitae(FormCollection form)
        {
            GetAdminData();
            IEnumerable<NotAssignDocumentsViewModel> NotAssignDocumentsViewModel = new List<NotAssignDocumentsViewModel>();
            var selectedIds = form.GetValues("selectedDocument");
            selectedIds = selectedIds.Where(id => id != "false").ToArray();

            if (selectedIds != null)
                NotAssignDocumentsViewModel = GetNotAssignDocuments(selectedIds);

            MultiNotAssignDocuments MultiNotAssignDocuments = new MultiNotAssignDocuments();
            MultiNotAssignDocuments.NotAssignDocumentsViewModel = NotAssignDocumentsViewModel;
            MultiNotAssignDocuments.AssignedDocument = new AssignedDocument();

            return View("SelectReviewer", MultiNotAssignDocuments);
        }

        [HttpPost]
        [Authorize(Roles = "Administrators")]
        public ActionResult SelectReviewer(MultiNotAssignDocuments MultiNotAssignDocuments)
        {
            GetAdminData();
            var NotAssignDocumentsViewModel = MultiNotAssignDocuments.NotAssignDocumentsViewModel;
            List<AssignedDocument> AssignedDocumentlist = new List<AssignedDocument>();
            foreach (var NotAssignedDocument in NotAssignDocumentsViewModel)
            {
                var currentAssignedDocument = GetAssignedDocument(Int32.Parse(NotAssignedDocument.DocumentId));
                currentAssignedDocument.ReviewerUserId = MultiNotAssignDocuments.AssignedDocument.ReviewerUserId;
                AssignedDocumentlist.Add(currentAssignedDocument);
            }
            AssignDocuments(AssignedDocumentlist);
            return RedirectToAction("AssignCurriculumVitae");
        }

        //[HttpPost]
        //[Authorize(Roles = "Administrators")]
        //public ActionResult AssignSelected(FormCollection form, AssignedDocument DocumentToAssign)
        //{
        //    List<AssignedDocument> AssignedDocumentlist = new List<AssignedDocument>();
        //    var StrselectedIds = DocumentToAssign.AdministratorUserId;
        //    if (StrselectedIds.Contains('#'))
        //    {
        //        var selectedIds = StrselectedIds.Split('#');
        //        if (selectedIds != null)
        //        {
        //            foreach (var id in selectedIds)
        //            {
        //                var currentAssignedDocument = GetAssignedDocument(Int32.Parse(id));
        //                //var currentAssignedDocument = GetAssignedDocument(EncryptionHelper.Decrypt(id));
        //                currentAssignedDocument.ReviewerUserId = DocumentToAssign.ReviewerUserId;
        //                AssignedDocumentlist.Add(currentAssignedDocument);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        var currentAssignedDocument = GetAssignedDocument(DocumentToAssign.DocumentId);
        //        //var currentAssignedDocument = GetAssignedDocument(EncryptionHelper.Decrypt(id));
        //        currentAssignedDocument.ReviewerUserId = DocumentToAssign.ReviewerUserId;
        //        AssignedDocumentlist.Add(currentAssignedDocument);
        //    }

        //    AssignDocuments(AssignedDocumentlist);
        //    return RedirectToAction("AssignCurriculumVitae");
        //}

        public AssignedDocument GetAssignedDocument(int DocumentId)
        {
            AssignedDocument DocumentToAssign = new AssignedDocument();
            using (var context = new ApplicationDbContext())
            {
                var DocumentObject = context.Documents.Where(doc => doc.DocumentId == DocumentId).Include("User").FirstOrDefault();
                if (DocumentObject != null)
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