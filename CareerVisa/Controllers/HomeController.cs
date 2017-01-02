using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CareerVisa.Models;

namespace CareerVisa.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.IsInRole("Administrators"))
                return RedirectToAction("Index", "Administrators");
            else
                return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Why()
        {
            return View();
        }

        public PartialViewResult Features()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return PartialView("_Features", db.Features.ToList());
        }
        public PartialViewResult Functionality()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return PartialView("_Functionality", db.Functionalities.ToList());
        }

        public ViewResult FunctionalityDetails(int FunctionalityId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var Functionality = db.Functionalities.Where(f => f.FunctionalityId == FunctionalityId).First();

            return View(Functionality);
        }

    }
}