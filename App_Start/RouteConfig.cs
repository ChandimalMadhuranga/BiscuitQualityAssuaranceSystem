using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace BiscuitQualityAssuaranceSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }
            //defaults: new { controller = "Shift_Manager", action = "Index", id = UrlParameter.Optional }
            //defaults: new { controller = "Products", action = "Index", id = UrlParameter.Optional }
            //defaults: new { controller = "Quality_Parameter", action = "Index", id = UrlParameter.Optional }
            // defaults: new { controller = "Quality_Checker", action = "Index", id = UrlParameter.Optional }
            // defaults: new { controller = "Complaints", action = "Index", id = UrlParameter.Optional }
            // defaults: new { controller = "Decisions", action = "Index", id = UrlParameter.Optional }
            // defaults: new { controller = "Categories", action = "Index", id = UrlParameter.Optional }
            // defaults: new { controller = "Product_Plan", action = "Index", id = UrlParameter.Optional }

            );
        }
    }
}
