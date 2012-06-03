using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    public class AuthenticationHandler : DelegatingHandler
    {
        HttpAuthentication _authN;

        public AuthenticationHandler(AuthenticationConfiguration configuration)
        {
            _authN = new HttpAuthentication(configuration);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // try to authenticate
                // returns an anonymous principal if no credential was found
                var principal = _authN.Authenticate(request);

                // run claims transformation
                if (_authN.Configuration.ClaimsAuthenticationManager != null)
                {
                    principal = _authN.Configuration.ClaimsAuthenticationManager.Authenticate(request.RequestUri.AbsoluteUri, principal);
                }

                // set the principal
                Thread.CurrentPrincipal = principal;
            }
            catch
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                    SetAuthenticateHeader(response);

                    return response;
                });
            }

            return base.SendAsync(request, cancellationToken).ContinueWith(
                (task) =>
                {
                    var response = task.Result;

                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        SetAuthenticateHeader(response);
                    }

                    return response;
                });
        }

        private void SetAuthenticateHeader(HttpResponseMessage response)
        {
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(_authN.Configuration.DefaultAuthenticationScheme));
        }
    }
}