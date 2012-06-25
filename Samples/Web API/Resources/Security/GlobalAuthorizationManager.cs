using System.Security.Claims;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel.Authorization;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Resources.Security
{
    public class GlobalAuthorizationManager : GlobalAuthorization
    {
        public GlobalAuthorizationManager(DefaultPolicy policy = DefaultPolicy.Deny)
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
