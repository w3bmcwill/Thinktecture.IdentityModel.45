/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Services;
using System.IdentityModel.Services.Tokens;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
                    var principal = AuthenticateAuthorizationHeader(authZ.Scheme, authZ.Parameter);

                    if (principal.Identity.IsAuthenticated)
                    {
                        return principal;
                    }
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
                if (request.RequestUri != null && !string.IsNullOrWhiteSpace(request.RequestUri.Query))
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

            if (queryString != null)
            {
                foreach (var param in queryString)
                {
                    if (Configuration.TryGetQueryStringMapping(param.Key, out handlers))
                    {
                        return InvokeHandler(handlers, param.Value, null);
                    }
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

            var query = uri.Query;
            if (query == null)
            {
                return null;
            }

            if (query[0] == '?')
            {
                query = query.Substring(1);
            }

            if (query.Contains('&'))
            {
                pairs = query.Split('&');
            }
            else
            {
                pairs = new string[] { query };
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

        #region Session
        public SessionSecurityToken CreateSessionToken(ClaimsPrincipal principal)
        {
            var token = new SessionSecurityToken(principal, TimeSpan.FromHours(8));

            // configure token

            return token;
        }

        public void IssueSessionToken(SessionSecurityToken token, HttpResponseMessage response)
        {
            var handler = new MachineKeySessionSecurityTokenHandler();
            var bytes = handler.WriteToken(token);

            WriteInternal(bytes, "auth", "/", "", token.ValidTo, true, true, response);

        }

        public const int DefaultChunkSize = 2000;
        internal void WriteInternal(byte[] value, string name, string path, string domain, DateTime expirationTime, bool secure, bool httpOnly, HttpResponseMessage response)
        {
            string cookieValue = Convert.ToBase64String(value);

            //this.DeleteInternal(name, path, domain, requestCookies, responseCookies);

            var cookies = new List<CookieHeaderValue>();
            foreach (KeyValuePair<string, string> keyValuePair in this.GetCookieChunks(name, cookieValue))
            {
                CookieHeaderValue cookieHeader = new CookieHeaderValue(keyValuePair.Key, keyValuePair.Value);

                cookieHeader.Secure = secure;
                cookieHeader.HttpOnly = httpOnly;
                cookieHeader.Path = path;

                if (!string.IsNullOrEmpty(domain))
                    cookieHeader.Domain = domain;
                if (expirationTime != DateTime.MinValue)
                    cookieHeader.Expires = expirationTime;

                cookies.Add(cookieHeader);

                //if (System.IdentityModel.Services.DiagnosticUtility.ShouldTrace(TraceEventType.Information))
                //    TraceUtility.TraceEvent(TraceEventType.Information, 786438, (string)null, (TraceRecord)new ChunkedCookieHandlerTraceRecord(ChunkedCookieHandlerTraceRecord.Action.Writing, cookie, cookie.Path), (object)null);
            }

            response.Headers.AddCookies(cookies);
        }

        private IEnumerable<KeyValuePair<string, string>> GetCookieChunks(string baseName, string cookieValue)
        {
            int chunksRequired = CeilingDivide(cookieValue.Length, DefaultChunkSize);

            //if (chunksRequired > 20 && System.IdentityModel.Services.DiagnosticUtility.ShouldTrace(TraceEventType.Warning))
            //    TraceUtility.TraceString(TraceEventType.Warning, System.IdentityModel.Services.SR.GetString("ID8008"), new object[0]);

            for (int i = 0; i < chunksRequired; ++i)
                yield return new KeyValuePair<string, string>(GetChunkName(baseName, i), cookieValue.Substring(i * DefaultChunkSize, Math.Min(cookieValue.Length - i * DefaultChunkSize, DefaultChunkSize)));
        }

        public static int CeilingDivide(int dividend, int divisor)
        {
            int num = dividend % divisor;
            int num2 = dividend / divisor;
            if (num > 0)
            {
                num2++;
            }
            return num2;
        }



        private string GetChunkName(string baseName, int chunkIndex)
        {
            if (chunkIndex != 0)
                return baseName + chunkIndex.ToString((IFormatProvider)CultureInfo.InvariantCulture);
            else
                return baseName;
        }
        #endregion
    }
}
