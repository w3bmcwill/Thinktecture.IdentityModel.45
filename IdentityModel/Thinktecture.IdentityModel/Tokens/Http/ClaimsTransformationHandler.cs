using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    public class ClaimsTransformationHandler : DelegatingHandler
    {
        ClaimsAuthenticationManager _transfomer;

        public ClaimsTransformationHandler()
        {
            _transfomer = FederatedAuthentication.FederationConfiguration.IdentityConfiguration.ClaimsAuthenticationManager;
        }

        public ClaimsTransformationHandler(ClaimsAuthenticationManager transformer)
        {
            _transfomer = transformer;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Thread.CurrentPrincipal = _transfomer.Authenticate(request.RequestUri.AbsoluteUri, ClaimsPrincipal.Current);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
