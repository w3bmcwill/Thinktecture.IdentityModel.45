using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Claims
{
    public static class AnonymousClaimsPrincipal
    {
        public static ClaimsPrincipal Create()
        {
            var anonId = new ClaimsIdentity();
            var anonPrincipal = new ClaimsPrincipal(anonId);

            return anonPrincipal;
        }
    }
}
