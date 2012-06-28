using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Thinktecture.IdentityModel.Http.Cors;
using Thinktecture.IdentityModel.Http.Cors.IIS;

namespace CorsSampleIIS
{
    public class Global : System.Web.HttpApplication
    {
        void ConfigureCors(CorsConfiguration corsConfig)
        {
            corsConfig
                .ForResources("~/Handler1.ashx")
                .ForOrigins("http://foo.com", "http://bar.com")
                .AllowAll();

            corsConfig
                .ForResources("~/Handler1.ashx")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST");
            
            corsConfig
                .ForResources("~/Handler2.ashx")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST")
                .AllowCookies()
                .AllowResponseHeaders("Foo");

            corsConfig
                .ForResources("~/Handler3.ashx")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST", "PUT")
                .AllowRequestHeaders("Content-Type");

            corsConfig
                .ForResources("~/Handler4.ashx")
                .ForOrigins("http://localhost")
                .AllowAllMethods()
                .AllowCookies()
                .AllowRequestHeaders("Content-Type", "Foo", "Authorization")
                .AllowResponseHeaders("Foo");
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ConfigureCors(UrlBasedCorsConfiguration.Configuration);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}