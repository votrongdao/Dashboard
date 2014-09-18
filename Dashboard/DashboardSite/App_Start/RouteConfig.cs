using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DashboardSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("NotFound", "{whatever}/{dotnetfile}.aspx", new { controller = "Home", action = "NotFound" });
            routes.MapRoute(
                name: "ResetPwdRoute",
                url: "Account/ResetPassword/{id}/{usr}",
                defaults: new { controller = "Account", action = "ResetPassword", id = UrlParameter.Optional, usr = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                //defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );            
        }
    }
}