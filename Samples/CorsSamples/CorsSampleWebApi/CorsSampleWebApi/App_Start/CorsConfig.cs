using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors.WebApi;

namespace WebApiSample.App_Start
{
    public class CorsConfig
    {
        public static void RegisterCors(HttpConfiguration httpConfig)
        {
            WebApiCorsConfiguration corsConfig = new WebApiCorsConfiguration();
            corsConfig.RegisterGlobal(httpConfig);

            corsConfig
                .ForResources("Values1")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST");

            corsConfig
                .ForResources("Values2")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST")
                .AllowCookies()
                .AllowResponseHeaders("Foo");

            corsConfig
                .ForResources("Values3")
                .ForOrigins("http://localhost")
                .AllowMethods("GET", "POST", "PUT")
                .AllowRequestHeaders("Content-Type");

            corsConfig
                .ForResources("Values4")
                .ForOrigins("http://localhost")
                .AllowAllMethods()
                .AllowCookies()
                .AllowRequestHeaders("Content-Type", "Foo", "Authorization")
                .AllowResponseHeaders("Foo");
        }
    }
}