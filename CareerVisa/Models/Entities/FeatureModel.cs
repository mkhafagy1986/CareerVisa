using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class Feature
    {
        [Key]
        public int FeatureId { get; set; }
        public string FeatureName { get; set; }
        public string FeatureIconName { get; set; }
        public string FeatureDescribtion { get; set; }
    }
}