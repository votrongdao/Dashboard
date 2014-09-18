using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HUDHealthcarePortal.Model;

namespace HUDHealthCarePortal.Controllers
{
    public class ReviewsController : Controller
    {
        //
        // GET: /Reviews/

        public ActionResult Index()
        {
            //var model = from r in _reviews
            //            orderby r.Country
            //            select r;
            return View();
        }

        //
        // GET: /Reviews/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Reviews/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Reviews/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Reviews/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Reviews/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Reviews/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Reviews/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //static List<ResturantReview> _reviews = new List<ResturantReview>
        //{
        //    new ResturantReview {
        //        Id = 1,
        //        Name = "Cinnamon Club",
        //        City="London",
        //        Country = "UK",
        //        Ratings = 10,
        //    },
        //    new ResturantReview {
        //            Id = 2,
        //            Name = "Marrakesh",
        //            City="D.C",
        //            Country = "USA",
        //            Ratings = 10,
        //    },
        //    new ResturantReview {
        //            Id = 3,
        //            Name = "The House of Elliot",
        //            City="Ghent",
        //            Country = "Belgium",
        //            Ratings = 10
           
        //    }
        //};
    }
}
