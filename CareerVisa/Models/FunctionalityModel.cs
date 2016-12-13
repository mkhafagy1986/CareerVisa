using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTutorial.Models
{
    public class Functionality
    {
        public int FunctionalityId { get; set; }
        public string FunctionalityName { get; set; }
        public string FunctionalityDescription { get; set; }
        public string FunctionalityImagePath { get; set; }
    }
}