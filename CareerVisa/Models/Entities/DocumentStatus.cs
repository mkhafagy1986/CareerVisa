﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class DocumentStatus
    {
        [Key]
        public int DocumentStatusId { get; set; }
        public string DocumentStatusDescription { get; set; }
    }
}