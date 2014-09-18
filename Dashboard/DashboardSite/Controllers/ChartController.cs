using DashboardSite.BusinessService;
using DashboardSite.BusinessService.Interface;
using DashboardSite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DashboardSite.Controllers
{
    public class ChartItem
    {
        public ChartItem(string category, int count, string color)
        {
            Category = category;
            Count = count;
            Color = color;
        }
        public string Category { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
    }

    [Authorize]
    public class ChartController : Controller
    {
        //IUploadDataManager uploadDataMgr;

        public ChartController() //: this(new UploadDataManager())
        {

        }

        public ActionResult Index()
        {
            return View("PieChart");
        }

        public ActionResult PieChart()
        {
            return View();
        }

        public ActionResult BarChart()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult GetPieChart()
        //{
        //    int[] ranges = { 4, 8, 12 };

        //    var query = from value in uploadDataMgr.GetUploadedLive().ToList()
        //                    .Where(p => p.ScoreTotal != null)
        //                    .Select(p => p.ScoreTotal)
        //                group value by ranges.Where(x => value >= x)
        //                                     .DefaultIfEmpty()
        //                                     .Last() into groups
        //                select new { Key = groups.Key, Values = groups };
        //    Dictionary<string, int> dictRiskCat = new Dictionary<string, int>();
        //    foreach (var group in query)
        //    {
        //        if (group.Key == 0)
        //            dictRiskCat.Add("Low Risk", group.Values.Count());
        //        else if (group.Key == 4)
        //            dictRiskCat.Add("Medium Risk", group.Values.Count());
        //        else if (group.Key == 8)
        //            dictRiskCat.Add("High Risk", group.Values.Count());
        //    }

        //    var key = new Chart(width: 300, height: 300)
        //        .AddTitle("Risk Summary")
        //        .AddSeries(
        //        chartType: "Pie",
        //        name: "Risk Summary",
        //        xValue: dictRiskCat.Keys,
        //        yValues: dictRiskCat.Values);

        //    return File(key.ToWebImage().GetBytes(), "image/jpeg");
        //}

        //[HttpGet]
        //public ActionResult GetBarChart()
        //{
        //    int[] ranges = { 4, 8, 12 };

        //    var query = from value in uploadDataMgr.GetUploadedLive().ToList()
        //                    .Where(p => p.ScoreTotal != null)
        //                    .Select(p => p.ScoreTotal)
        //                group value by ranges.Where(x => value >= x)
        //                                     .DefaultIfEmpty()
        //                                     .Last() into groups
        //                select new { Key = groups.Key, Values = groups };
        //    Dictionary<string, int> dictRiskCat = new Dictionary<string, int>();
        //    foreach (var group in query)
        //    {
        //        if (group.Key == 0)
        //            dictRiskCat.Add("Low Risk", group.Values.Count());
        //        else if (group.Key == 4)
        //            dictRiskCat.Add("Medium Risk", group.Values.Count());
        //        else if (group.Key == 8)
        //            dictRiskCat.Add("High Risk", group.Values.Count());
        //    }

        //    var key = new Chart(width: 300, height: 300)
        //        .AddTitle("Risk Summary")
        //        .AddSeries(
        //        chartType: "Bar",
        //        name: "Risk Summary",
        //        xValue: dictRiskCat.Keys,
        //        yValues: dictRiskCat.Values);

        //    //var theme = new System.Web.Helpers.ChartTheme();
        //    return File(key.ToWebImage().GetBytes(), "image/jpeg");
        //}        

        //[HttpPost]
        //public ActionResult GetGoolgeChart()
        //{
        //    //int[] ranges = { 11, 15, 15000 };
        //    int[] ranges = { 10, 20, 15000 };
        //    var projectList = uploadDataMgr.GetUploadedLive().ToList();
        //    var query = from value in projectList
        //                    .Where(p => p.ScoreTotal != null)
        //                    .Select(p => p.ScoreTotal)
        //                group value by ranges.Where(x => value >= x)
        //                                     .DefaultIfEmpty()
        //                                     .Last() into groups
        //                select new { Key = groups.Key, Values = groups };
        //    List<ChartItem> results = new List<ChartItem>();
        //    foreach (var group in query)
        //    {
        //        if (group.Key == 0)
        //            results.Add(new ChartItem("Low Risk", group.Values == null ? 0 : group.Values.Count(), "#92D050"));
        //        else if (group.Key == 10)
        //            results.Add(new ChartItem("Medium Risk", group.Values == null ? 0 : group.Values.Count(), "#FFFF00"));
        //        else if (group.Key == 20)
        //            results.Add(new ChartItem("High Risk", group.Values == null ? 0 : group.Values.Count(), "#FF7C80"));
        //    }

        //    return Json(results, JsonRequestBehavior.AllowGet);
        //}

        //[HttpGet]
        //public ActionResult GetUploadStatusChart()
        //{
        //    int[] ranges = { 10, 20, 15000 };
        //    var projectList = uploadDataMgr.GetUploadedLive().ToList();
        //    var query = from value in projectList
        //                    .Where(p => p.ScoreTotal != null)
        //                    .Select(p => p.ScoreTotal)
        //                group value by ranges.Where(x => value >= x)
        //                                     .DefaultIfEmpty()
        //                                     .Last() into groups
        //                select new { Key = groups.Key, Values = groups };
        //    List<ChartItem> results = new List<ChartItem>();
        //    foreach (var group in query)
        //    {
        //        if (group.Key == 0)
        //            results.Add(new ChartItem("Low Risk", group.Values == null ? 0 : group.Values.Count(), "#92D050"));
        //        else if (group.Key == 10)
        //            results.Add(new ChartItem("Medium Risk", group.Values == null ? 0 : group.Values.Count(), "#FFFF00"));
        //        else if (group.Key == 20)
        //            results.Add(new ChartItem("High Risk", group.Values == null ? 0 : group.Values.Count(), "#FF7C80"));
        //    }

        //    return Json(results, JsonRequestBehavior.AllowGet);
        //}
    }
}

