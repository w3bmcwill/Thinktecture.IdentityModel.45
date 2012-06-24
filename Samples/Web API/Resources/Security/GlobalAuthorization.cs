using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel.WebApi;

namespace Resources.Security
{
    public class GlobalAuthorization : GlobalAuthorizationManager
    {
        public GlobalAuthorization(DefaultPolicy policy = DefaultPolicy.Deny)
            : base(policy)
        { }

        // global authorization rules
        protected override bool Default(HttpActionContext context)
        {
            var principal = ClaimsPrincipal.Current;

            // demand a name claim
            return principal.HasClaim(c => c.Type == ClaimTypes.Name);
        }

        // authorization rules for consultants controller
        public bool ConsultantsAuthorization(HttpActionContext context)
        {
            return true;
        }
    }
}
