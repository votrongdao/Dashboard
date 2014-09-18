using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using HUDHealthcarePortal.Model;
using System.Configuration;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;
using HUDHealthcarePortal.Core;
using HUDHealthcarePortal.BusinessService;
using MvcSiteMapProvider;
using HUDHealthcarePortal.BusinessService.Interface;

namespace HUDHealthCarePortal.Controllers
{
    [Authorize]
    public class ExcelUploadView_Controller : Controller
    {
        IUploadDataManager uploadDataMgr;

        public ExcelUploadView_Controller() : this(new UploadDataManager())
        {

        }

        public ExcelUploadView_Controller(IUploadDataManager uploadDataManager)
        {
            uploadDataMgr = uploadDataManager;
        }
            List<ExcelUploadView_Model> inventoryList = new List<ExcelUploadView_Model>();
            
            [HttpGet]
            [MvcSiteMapNode(Title = "Project Detail", DynamicNodeProvider = "HUDHealthcarePortal.Helpers.ProjectDetailDynamicNodeProvider, HUDHealthcarePortal")]
            public ActionResult ProjectDetail(int id)
            {
                var model = uploadDataMgr.GetProjectDetailByID(id);
                   
                return View(model);
            }
            
            [HttpGet]
            public ActionResult ViewExcelUpload()
            {
                var uploadedData = uploadDataMgr
                    .GetUploadDataByRole(UserPrincipal.Current.UserId, UserPrincipal.Current.UserRole).ToList();
                return View("ViewExcelUploadDetail", uploadedData);
            }

            [MvcSiteMapNode(Title = "Upload Detail", ParentKey = "DetailId", Key="OneUploadId", PreservedRouteParameters="OneUploadId")]
            public ActionResult ViewExcelUploadDetail(DateTime dateInserted)
            {
                var uploadedData = uploadDataMgr.GetUploadDataByTime(dateInserted).ToList();

                return View(uploadedData);
            }

            [HttpPost]
            [MvcSiteMapNode(Title = "Risk Detail", PreservedRouteParameters = "category", ParentKey = "RiskSum", Key = "RiskDetailId")]
            public ActionResult ViewExcelUploadsByCategory(string category)
            {
                var uploadedData = uploadDataMgr.GetUploadDataByCategory(category).ToList();
                return View("ViewExcelUploadDetail", uploadedData);
            }

            public ActionResult ReportExcelUpload()
            {
                var model = uploadDataMgr.GetDataUploadSummaryByUserId(UserPrincipal.Current.UserId).ToList();
                return View(model);
            }
    }
}
