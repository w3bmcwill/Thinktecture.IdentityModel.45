using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.Http.Cors.WebAPI;

namespace Resources.Configuration
{
    public class CorsConfig
    {
        public static void RegisterGlobal(HttpConfiguration httpConfig)
        {
            var corsConfig = new WebAPICorsConfiguration();
            corsConfig.RegisterGlobal(httpConfig);
            corsConfig.AllowAll();
        }  
    }
}
