using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public class HttpClaimsAuthorizationManager : IAuthorizationManager
    {
        ClaimsAuthorizationManager _authZ;

        public HttpClaimsAuthorizationManager()
        {
            _authZ = FederatedAuthentication.FederationConfiguration
                                            .IdentityConfiguration
                                            .ClaimsAuthorizationManager;
        }

        public HttpClaimsAuthorizationManager(ClaimsAuthorizationManager authorizationManager)
        {
            _authZ = authorizationManager;
        }

        public bool CheckAccess(HttpActionContext context)
        {
            var authZcontext = new HttpAuthorizationContext(context);
            return _authZ.CheckAccess(authZcontext);
        }
    }
}
