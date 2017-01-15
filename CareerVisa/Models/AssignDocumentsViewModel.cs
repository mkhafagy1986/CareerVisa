using CareerVisa.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class NotAssignDocumentsViewModel
    {
        string _DocumentId;
        string _DocumentOwnerUserId;
        EncryptionHelper EncryptionHelper;
        public NotAssignDocumentsViewModel()
        {
            EncryptionHelper = new EncryptionHelper(ConfigurationManager.AppSettings["EncryptionKey"]);
        }
        public string DocumentOwnerUsername { get; set; }
        public string DocumentOwnerUserId
        {
            get
            {
                //return EncryptionHelper.Decrypt(_DocumentOwnerUserId);
                return _DocumentOwnerUserId;
            }
            set
            {
                _DocumentOwnerUserId = EncryptionHelper.Encrypt(value);
            }
        }
        public string DocumentId
        {
            get
            {
                return _DocumentId;
                //return EncryptionHelper.Decrypt(_DocumentId);
            }
            set
            {
                _DocumentId = EncryptionHelper.Encrypt(value);
            }
        }
        public string DocumentDescription { get; set; }
        public string DocumentStatus { get; set; }
        public int DocumentStatusId { get; set; }
        public string DocumentType { get; set; }
        public int DocumentTypeId { get; set; }
        public DateTime DocumentUploadDate { get; set; }
        public string ReviewerUserId { get; set; }
        public string ReviewerUserName { get; set; }
        public DateTime AssignDate { get; set; }
    }
}