using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Claims
{
    public static class Principal
    {
        public static ClaimsPrincipal Anonymous
        {
            get
            {
                {
                    var anonId = new ClaimsIdentity();
                    var anonPrincipal = new ClaimsPrincipal(anonId);

                    return anonPrincipal;
                }
            }
        }

        public static ClaimsPrincipal Create(string authenticationType, params Claim[] claims)
        {
            return new ClaimsPrincipal(new ClaimsIdentity(claims, authenticationType));
        }
    }
}
