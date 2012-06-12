using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Claims;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.Samples;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration(Constants.SelfHostBaseAddress);
            var a = Assembly.Load("resources");

            ConfigureApis(config);

            config.Routes.MapHttpRoute(
                "API Default",
                "{controller}/{id}",
                new { id = RouteParameter.Optional });

            var server = new HttpSelfHostServer(config);

            server.OpenAsync().Wait();

            Console.WriteLine("Server is running.");
            Console.ReadLine();

        }

        private static void ConfigureApis(HttpSelfHostConfiguration configuration)
        {
            configuration.MessageHandlers.Add(new AuthenticationHandler(CreateConfiguration()));
        }

        public static AuthenticationConfiguration CreateConfiguration()
        {
            var config = new AuthenticationConfiguration
            {
                DefaultAuthenticationScheme = "Basic",
                InheritHostClientIdentity = false
            };

            #region Basic Authentication
            config.AddBasicAuthentication((userName, password) => userName == password);
            #endregion

            #region SimpleWebToken
            config.AddSimpleWebToken(
                issuer:     "http://identity.thinktecture.com/trust",
                audience:   Constants.Realm,
                signingKey: Constants.IdSrvSymmetricSigningKey,
                options:    AuthenticationOptions.ForAuthorizationHeader("IdSrv"));
            #endregion

            #region JsonWebToken
            config.AddJsonWebToken(
                issuer:     "http://selfissued.test",
                audience:   Constants.Realm,
                signingKey: Constants.IdSrvSymmetricSigningKey,
                options:    AuthenticationOptions.ForAuthorizationHeader("JWT"));
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
                issuer:     "https://" + Constants.ACS + "/",
                audience:   Constants.Realm,
                signingKey: Constants.AcsSymmetricSigningKey,
                options:    AuthenticationOptions.ForAuthorizationHeader("ACS"));
            #endregion

            #region AccessKey
            var handler = new SimpleSecurityTokenHandler(token =>
            {
                if (ObfuscatingComparer.IsEqual(token, "accesskey123"))
                {
                    return IdentityFactory.Create("Custom",
                        new Claim("customerid", "123"),
                        new Claim("email", "foo@customer.com"));
                }

                return null;
            });

            config.AddAccessKey(handler, AuthenticationOptions.ForQueryString("key"));
            #endregion

            return config;
        }
    }
}
