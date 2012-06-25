using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors.WebApi;

namespace Resources.Configuration
{
    public class CorsConfig
    {
        public static void RegisterGlobal(HttpConfiguration httpConfig)
        {
            var corsConfig = new WebApiCorsConfiguration();
            corsConfig.RegisterGlobal(httpConfig);
            corsConfig.AllowAll();
        }  
    }
}
