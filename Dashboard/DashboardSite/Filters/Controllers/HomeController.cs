using HUDHealthcarePortal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace HUDHealthCarePortal.Controllers
{
    public class HomeController : Controller
    {
        
        //StateDb _db = new StateDb();

        //OdeToFoodDb _db = new OdeToFoodDb();

        //[HttpPost]
        //[AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NotFound()
        {
            ViewBag.Message = "This page is under construction";

            return View("_UnderConstruction");
        }

        protected override void Dispose(bool disposing)
        {
            //if (_db != null)
            //{
            //    _db.Dispose();
            //}
            //base.Dispose(disposing);
        }        

        public ActionResult Chapati()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Chapati(FormCollection Frm)
        {
            return RedirectToAction("Contact");
        }


    }
}
