using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class MultiNotAssignDocuments
    {
        IEnumerable<CareerVisa.Models.NotAssignDocumentsViewModel> _NotAssignDocumentsViewModel;
        
        public IEnumerable<CareerVisa.Models.NotAssignDocumentsViewModel> NotAssignDocumentsViewModel
        {
            get
            {
                return _NotAssignDocumentsViewModel;
            }
            set
            {
                _NotAssignDocumentsViewModel = value;
            }
        }

        CareerVisa.Models.Entities.AssignedDocument _AssignedDocument;

        public CareerVisa.Models.Entities.AssignedDocument AssignedDocument
        {
            get
            {
                return _AssignedDocument;
            }
            set
            {
                _AssignedDocument = value;
            }
        }
    }
}