using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCTutorial.Controllers
{
    public class FeaturesController : Controller
    {
        // GET: Features
        public ActionResult Index()
        {
            return View();
        }
    }
}