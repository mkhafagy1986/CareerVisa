using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class UsersCountViewModel
    {
        public int EmployersCount { get; set; }
        public int JobSeekerCount { get; set; }
        public int NotAssignedCVsCount { get; set; }
        public int NotAssignedCoverLettersCount { get; set; }
    }
}