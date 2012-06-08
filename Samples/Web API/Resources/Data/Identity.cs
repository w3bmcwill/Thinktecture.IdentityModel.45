using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Resources
{
    public class Identity
    {
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public string ClrType { get; set; }

        public ClaimsDto Claims { get; set; }

        public Identity()
        { }

        public Identity(IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }

            ClrType = identity.GetType().FullName;

            if (!identity.IsAuthenticated)
            {
                IsAuthenticated = false;
                return;
            }

            Name = identity.Name;
            AuthenticationType = identity.AuthenticationType;
            IsAuthenticated = true;

            var claimsIdentity = identity as ClaimsIdentity;
            if (claimsIdentity != null)
            {
                Claims = new ClaimsDto();
                claimsIdentity.Claims.ToList().ForEach(c => Claims.Add(new ClaimDto
                {
                    ClaimType = c.Type,
                    Value = c.Value,
                    Issuer = c.Issuer,
                    OriginalIssuer = c.OriginalIssuer
                }));
            }
        }
    }

    public class ClaimsDto : List<ClaimDto>
    {
        public ClaimsDto()
        { }

        public ClaimsDto(IEnumerable<ClaimDto> claims)
            : base(claims)
        { }
    }

    public class ClaimDto
    {
        public string ClaimType { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
        public string OriginalIssuer { get; set; }
    }
}