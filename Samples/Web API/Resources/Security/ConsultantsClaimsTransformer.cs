using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;

namespace Resources.Security
{
    public class ConsultantsClaimsTransformer : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            return CreateClientIdentity(incomingPrincipal.Identity as ClaimsIdentity);
        }

        private ClaimsPrincipal CreateClientIdentity(ClaimsIdentity id)
        {
            var claims = new List<Claim>();

            // insert authentication method (windows, passsword, x509) as a claim
            var authMethod = id.Claims.SingleOrDefault(c => c.Type == ClaimTypes.AuthenticationMethod);

            if (authMethod == null)
            {
                authMethod = new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Unspecified);
            }

            // hard coded for demo purposes
            if (id.Name == "dominick")
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, id.Name),
                    new Claim(ClaimTypes.Role, "Users"),
                    new Claim(AppClaimTypes.ReportsTo, "christian"),
                    new Claim(ClaimTypes.Email, id.Name + "@thinktecture.com"),
                    authMethod
                };
            }
            else
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, id.Name),
                    new Claim(ClaimTypes.Role, "Users"),
                    new Claim(ClaimTypes.Email, id.Name + "@thinktecture.com"),
                    authMethod
                };
            }

            var claimsIdentity = new ClaimsIdentity(claims, "Local");
            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}
