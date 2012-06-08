using System;
using System.Collections.Generic;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Resources.Security;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.Samples;

namespace WebApiSecurity
{
    public class SecurityConfig
    {
        public static void ConfigureGlobal(HttpConfiguration globalConfig)
        {
            globalConfig.MessageHandlers.Add(new AuthenticationHandler(CreateConfiguration()));
            globalConfig.Filters.Add(new SecurityExceptionFilter());
        }

        public static AuthenticationConfiguration CreateConfiguration()
        {
            var config = new AuthenticationConfiguration
            {
                DefaultAuthenticationScheme = "Basic",
            };
            
            #region Basic Authentication
            config.AddBasicAuthentication((userName, password) => userName == password);
            #endregion

            #region SimpleWebToken
            config.AddSimpleWebToken(
                "http://identity.thinktecture.com/trust",
                Constants.Realm,
                Constants.IdSrvSymmetricSigningKey,
                AuthenticationOptions.ForAuthorizationHeader("IdSrv"));
            #endregion

            #region JsonWebToken
            config.AddJsonWebToken(
                "http://selfissued.test",
                Constants.Realm,
                Constants.IdSrvSymmetricSigningKey,
                AuthenticationOptions.ForAuthorizationHeader("JWT"));
            #endregion

            #region IdentityServer SAML
            var idsrvRegistry = new ConfigurationBasedIssuerNameRegistry();
            idsrvRegistry.AddTrustedIssuer("A1EED7897E55388FCE60FEF1A1EED81FF1CBAEC6", "Thinktecture IdSrv");

            var idsrvConfig = new SecurityTokenHandlerConfiguration();
            idsrvConfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(Constants.Realm));
            idsrvConfig.IssuerNameRegistry = idsrvRegistry;
            idsrvConfig.CertificateValidator = X509CertificateValidator.None;

            config.AddSaml2(idsrvConfig, AuthenticationOptions.ForAuthorizationHeader("IdSrvSaml"));
            #endregion

            #region ACS SWT
            config.AddSimpleWebToken(
                "https://" + Constants.ACS + "/",
                Constants.Realm,
                Constants.AcsSymmetricSigningKey,
                AuthenticationOptions.ForAuthorizationHeader("ACS"));
            #endregion

            #region AccessKey
            var handler = new SimpleSecurityTokenHandler("my access key", token =>
                {
                    if (ObfuscatingComparer.IsEqual(token, "accesskey123"))
                    {
                        return new ClaimsIdentity(new Claim[]
                    {
                        new Claim("customerid", "123")
                    }, "Custom");
                    }

                    return null;
                });

            config.AddAccessKey(handler, AuthenticationOptions.ForQueryString("key"));
            #endregion

            return config;
        }
    }
}