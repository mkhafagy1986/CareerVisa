using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class Configuration
    {
        public int ConfigurationId { get; set; }
        public int ConfigurationType { get; set; }
        public string ConfigurationKey { get; set; }
        public string ConfigurationValue { get; set; }
    }
}