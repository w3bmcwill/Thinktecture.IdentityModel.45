using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Thinktecture.IdentityModel.Http.Cors.MVC
{
    public class CorsAuthorizationFilterAttribute : AuthorizeAttribute
    {
        public CorsAuthorizationFilterAttribute()
        {
        }

        Type configType;
        public CorsAuthorizationFilterAttribute(Type configType)
        {
            this.configType = configType;
        }

        CorsConfiguration Configuration
        {
            get
            {
                if (this.configType != null)
                {
                    CorsConfiguration ret = System.Web.Mvc.DependencyResolver.Current.GetService(configType) as CorsConfiguration;
                    if (ret == null)
                    {
                        ret = Activator.CreateInstance(configType) as CorsConfiguration;
                    }
                    return ret;
                }
                return MVCConfiguration.Configuration;
            }
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var httpRequest = new MVCRequest(httpContext.Request);
            var accessRequest = new CorsAccessRequest(httpRequest);
            var accessResponse = MVCConfiguration.Configuration.Engine.CheckAccess(accessRequest);
            if (accessResponse != null)
            {
                var response = httpContext.Response;
                var httpResponse = new MVCResponse(response);
                accessResponse.WriteResponse(httpResponse);
            }

            return !accessRequest.IsCorsPreflight;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}
