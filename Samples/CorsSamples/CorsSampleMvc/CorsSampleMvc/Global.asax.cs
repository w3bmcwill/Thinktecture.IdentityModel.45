using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Thinktecture.IdentityModel.Http.Cors;
using Thinktecture.IdentityModel.Http.Cors.Mvc;

namespace CorsSampleMvc
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        private void RegisterCors(MvcCorsConfiguration corsConfig)
        {
            corsConfig
                .ForResources("Values1.GetData")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST");

            corsConfig
                .ForResources("Values2.GetData")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST")
                .AllowCookies()
                .AllowResponseHeaders("Foo");

            corsConfig
                .ForResources("Values3.GetData")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST", "PUT")
                .AllowRequestHeaders("Content-Type");

            corsConfig
                .ForResources("Values4.GetData")
                .ForOrigins("http://localhost")
                .AllowAllMethods()
                .AllowCookies()
                .AllowRequestHeaders("Content-Type", "Foo", "Authorization")
                .AllowResponseHeaders("Foo");
        }
        
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            RegisterCors(MvcCorsConfiguration.Configuration);
        }
    }
}