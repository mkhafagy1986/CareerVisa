using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models.Entities
{
    public class LoggedInUser
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public DateTime LoggingInDateTime { get; set; }
    }
}