using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Resources.Security
{
    public class ConsultantsAuthorizationManager : PerControllerAuthorization
    {
        IConsultantsRepository _repository;

        public ConsultantsAuthorizationManager(IConsultantsRepository repository)
        {
            _repository = repository;
        }

        protected override bool Default(HttpActionContext context)
        {
            var p = ClaimsPrincipal.Current;
            return p.HasClaim(AppClaimTypes.ReportsTo, "christian");
        }

        protected override bool Get(HttpActionContext context)
        {
            throw new System.NotImplementedException();
        }

        protected override bool Put(HttpActionContext context)
        {
            var principal = ClaimsPrincipal.Current;

            // if no id is specified, nothing to do here
            if (context.ControllerContext.RouteData.Values.ContainsKey("id"))
            {
                return CheckOwnership(int.Parse(context.ControllerContext.RouteData.Values["id"].ToString()), principal);
            }

            return true;
        }

        protected override bool Post(HttpActionContext context)
        {
            return true;
        }

        protected override bool Delete(HttpActionContext context)
        {
            var principal = ClaimsPrincipal.Current;

            // authorize based on authentication method
            if (!principal.HasClaim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.X509))
            {
                return false;
            }

            // if no id is specified, nothing to do here
            if (context.ControllerContext.RouteData.Values.ContainsKey("id"))
            {
                return CheckOwnership(int.Parse(context.ControllerContext.RouteData.Values["id"].ToString()), principal);
            }

            return true;
        }

        private bool CheckOwnership(int id, ClaimsPrincipal principal)
        {
            var oldConsultant = _repository.GetAll().FirstOrDefault(c => c.ID == id);

            if (oldConsultant == null)
            {
                return true;
            }

            // check if client is allowed to update consultant
            // only the record creator can update
            return oldConsultant.Owner.Equals(principal.Identity.Name);
        }
    }
}
