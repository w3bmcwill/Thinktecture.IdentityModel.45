using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Resources;
using Resources.Security;

namespace WebApiSecurity
{
    public class DependencyConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new AuthorizationDependencyResolver();
        }
    }
}