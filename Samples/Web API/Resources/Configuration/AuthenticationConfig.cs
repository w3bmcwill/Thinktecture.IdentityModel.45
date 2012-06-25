using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Web.Http;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Tokens.Http;

namespace Resources.Configuration
{
    public class AuthenticationConfig
    {
        public static void ConfigureGlobal(HttpConfiguration globalConfig)
        {
            globalConfig.MessageHandlers.Add(new AuthenticationHandler(CreateConfiguration()));
        }

        public static AuthenticationConfiguration CreateConfiguration()
        {
            var config = new AuthenticationConfiguration
            {
                DefaultAuthenticationScheme = "Basic",
                EnableSessionToken = true
            };

            #region BasicAuthentication
            config.AddBasicAuthentication((userName, password) => userName == password);
            #endregion

            #region SimpleWebToken
            config.AddSimpleWebToken(
                issuer: "http://identity.thinktecture.com/trust",
                audience: Constants.Realm,
                signingKey: Constants.IdSrvSymmetricSigningKey,
                options: AuthenticationOptions.ForAuthorizationHeader("IdSrv"));
            #endregion

            #region JsonWebToken
            config.AddJsonWebToken(
                issuer: "http://selfissued.test",
                audience: Constants.Realm,
                signingKey: Constants.IdSrvSymmetricSigningKey,
                options: AuthenticationOptions.ForAuthorizationHeader("JWT"));
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

            #region ADFS SAML
            var adfsRegistry = new ConfigurationBasedIssuerNameRegistry();
            adfsRegistry.AddTrustedIssuer("8EC7F962CC083FF7C5997D8A4D5ED64B12E4C174", "ADFS");

            var adfsConfig = new SecurityTokenHandlerConfiguration();
            adfsConfig.AudienceRestriction.AllowedAudienceUris.Add(new Uri(Constants.Realm));
            adfsConfig.IssuerNameRegistry = adfsRegistry;
            adfsConfig.CertificateValidator = X509CertificateValidator.None;

            config.AddSaml2(adfsConfig, AuthenticationOptions.ForAuthorizationHeader("AdfsSaml"));
            #endregion

            #region ACS SWT
            config.AddSimpleWebToken(
                issuer: "https://" + Constants.ACS + "/",
                audience: Constants.Realm,
                signingKey: Constants.AcsSymmetricSigningKey,
                options: AuthenticationOptions.ForAuthorizationHeader("ACS"));
            #endregion

            #region AccessKey
            config.AddAccessKey(token =>
            {
                if (ObfuscatingComparer.IsEqual(token, "accesskey123"))
                {
                    return Principal.Create("Custom",
                        new Claim("customerid", "123"),
                        new Claim("email", "foo@customer.com"));
                }

                return null;
            }, AuthenticationOptions.ForQueryString("key"));
            #endregion

            return config;
        }
    }
}
