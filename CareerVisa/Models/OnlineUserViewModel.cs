using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareerVisa.Models
{
    public class OnlineUserViewModel
    {
        public string ConnectionId { get; set; }
        public ApplicationUser User { get; set; }
    }
}