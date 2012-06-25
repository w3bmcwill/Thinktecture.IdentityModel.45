using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Resources.Configuration;

namespace WebApiSecurity
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            AuthenticationConfig.ConfigureGlobal(GlobalConfiguration.Configuration);
            DependencyConfig.Configure(GlobalConfiguration.Configuration);
            CorsConfig.RegisterGlobal(GlobalConfiguration.Configuration);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }   
    }
}