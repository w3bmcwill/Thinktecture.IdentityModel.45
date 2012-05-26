/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Claims;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    public class HttpAuthentication
    {
        public AuthenticationConfiguration Configuration { get; set; }

        public HttpAuthentication(AuthenticationConfiguration configuration)
        {
            Configuration = configuration;
        }

        public ClaimsPrincipal Authenticate(HttpRequestMessage request)
        {
            if (Configuration.HasAuthorizationHeaderMapping)
            {
                var authZ = request.Headers.Authorization;
                if (authZ != null)
                {
                    return AuthenticateAuthorizationHeader(authZ.Scheme, authZ.Parameter);
                }
            }

            if (Configuration.HasHeaderMapping)
            {
                if (request.Headers != null)
                {
                    var principal = AuthenticateHeaders(request.Headers);

                    if (principal.Identity.IsAuthenticated)
                    {
                        return principal;
                    }
                }
            }

            if (Configuration.HasQueryStringMapping)
            {
                if (request.RequestUri != null)
                {
                    var principal = AuthenticateQueryStrings(request.RequestUri);

                    if (principal.Identity.IsAuthenticated)
                    {
                        return principal;
                    }
                }
            }
            
            return AnonymousClaimsPrincipal.Create();
        }

        public ClaimsPrincipal Authenticate(Tuple<string, string> authorizationHeader, Dictionary<string, string> headers, Dictionary<string, string> queryString, X509Certificate2 clientCertificate)
        {
            if (Configuration.HasAuthorizationHeaderMapping)
            {
                if (authorizationHeader != null && !string.IsNullOrWhiteSpace(authorizationHeader.Item1))
                {
                    return AuthenticateAuthorizationHeader(authorizationHeader.Item1, authorizationHeader.Item2);
                }
            }

            if (Configuration.HasHeaderMapping)
            {
                if (headers != null && headers.Count > 0)
                {
                    var principal = AuthenticateHeaders(headers);

                    if (principal.Identity.IsAuthenticated)
                    {
                        return principal;
                    }
                }
            }

            if (Configuration.HasQueryStringMapping)
            {
                if (queryString != null && queryString.Count > 0)
                {
                    var principal = AuthenticateQueryStrings(queryString);

                    if (principal.Identity.IsAuthenticated)
                    {
                        return principal;
                    }
                }
            }

            if (Configuration.HasClientCertificateMapping)
            {
                var principal = AuthenticateClientCertificate(clientCertificate);

                if (principal.Identity.IsAuthenticated)
                {
                    return principal;
                }
            }

            return AnonymousClaimsPrincipal.Create();
        }

        public ClaimsPrincipal AuthenticateAuthorizationHeader(string scheme, string credential)
        {
            SecurityTokenHandlerCollection handlers;

            if (Configuration.TryGetAuthorizationHeaderMapping(scheme, out handlers))
            {
                return InvokeHandler(handlers, credential, null);
            }
            else
            {
                return AnonymousClaimsPrincipal.Create();
            }
        }

        

        public ClaimsPrincipal AuthenticateHeaders(Dictionary<string, string> headers)
        {
            SecurityTokenHandlerCollection handlers;

            foreach (var header in headers)
            {
                if (Configuration.TryGetHeaderMapping(header.Key, out handlers))
                {
                    return InvokeHandler(handlers, header.Value, null);
                }
            }

            return AnonymousClaimsPrincipal.Create();
        }

        public ClaimsPrincipal AuthenticateHeaders(HttpRequestHeaders headers)
        {
            SecurityTokenHandlerCollection handlers;

            foreach (var header in headers.AsEnumerable())
            {
                if (Configuration.TryGetHeaderMapping(header.Key, out handlers))
                {
                    return InvokeHandler(handlers, header.Value.First(), null);
                }
            }

            return AnonymousClaimsPrincipal.Create();
        }

        public ClaimsPrincipal AuthenticateQueryStrings(Dictionary<string, string> queryString)
        {
            SecurityTokenHandlerCollection handlers;

            foreach (var param in queryString)
            {
                if (Configuration.TryGetQueryStringMapping(param.Key, out handlers))
                {
                    return InvokeHandler(handlers, param.Value, null);
                }
            }

            return AnonymousClaimsPrincipal.Create();
        }

        public ClaimsPrincipal AuthenticateQueryStrings(Uri uri)
        {
            var qparams = CreateQueryStringDictionary(uri);

            return AuthenticateQueryStrings(qparams);
        }

        public ClaimsPrincipal AuthenticateClientCertificate(X509Certificate2 certificate)
        {
            SecurityTokenHandlerCollection handlers;
            var token = new X509SecurityToken(certificate);
            
            if (Configuration.TryGetClientCertificateMapping(out handlers))
            {
                var identity = handlers.First().ValidateToken(token);
                return new ClaimsPrincipal(identity);
            }

            return AnonymousClaimsPrincipal.Create();
        }

        private Dictionary<string, string> CreateQueryStringDictionary(Uri uri)
        {
            var dictionary = new Dictionary<string, string>();
            string[] pairs;

            if (uri.Query.Contains('&'))
            {
                pairs = uri.Query.Split('&');
            }
            else
            {
                pairs = new string[] { uri.Query };
            }

            foreach (var pair in pairs)
            {
                var parts = pair.Split('=');
                if (parts.Length == 2)
                {
                    dictionary.Add(parts[0], parts[1]);
                }
            }

            return dictionary;
        }

        private ClaimsPrincipal InvokeHandler(SecurityTokenHandlerCollection handlers, string tokenString, string identifier)
        {
            SecurityTokenHandler handler;

            if (handlers.Count == 1)
            {
                handler = handlers.First();
            }
            else
            {
                handler = handlers[identifier];
            }

            var token = handler.ReadToken(tokenString);
            return new ClaimsPrincipal(handler.ValidateToken(token));
        }
    }
}
