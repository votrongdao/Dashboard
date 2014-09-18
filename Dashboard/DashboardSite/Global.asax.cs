using Elmah;
using DashboardSite.BusinessService;
using DashboardSite.Core.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebMatrix.WebData;

namespace DashboardSite
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            AutoMapperInitialize.Initialize();

            // check if application pool is recycled
            //ExceptionManager.WriteToEventLog(string.Format("HCP Application starts at: {0}", DateTime.Now), "Application", System.Diagnostics.EventLogEntryType.Error);
        }

        public void ErrorLog_Filtering(object sender, ExceptionFilterEventArgs e)
        {
            // don't log file not found exception to elmah
            if (e.Exception.GetBaseException() is FileNotFoundException)
                e.Dismiss();
        }

        // prevent after logout, back button can view prior screens
        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}