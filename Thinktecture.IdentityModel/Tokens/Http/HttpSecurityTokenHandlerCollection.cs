using System;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tokens;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    public class HttpSecurityTokenHandlerCollection : SecurityTokenHandlerCollection
    {
        public HttpSecurityTokenHandlerCollection()
            : base()
        { }

        public void AddBasicAuthenticationHandler(BasicAuthenticationSecurityTokenHandler.ValidateUserNameCredentialDelegate validationDelegate)
        {
            var handler = new BasicAuthenticationSecurityTokenHandler(validationDelegate, "Basic");
            Add(handler);
        }

        public void AddSaml11SecurityTokenHandler(string identifier, SecurityTokenHandlerConfiguration configuration)
        {
            var saml = new HttpSamlSecurityTokenHandler(identifier)
            {
                Configuration = configuration
            };

            var encryption = new EncryptedSecurityTokenHandler()
            {
                Configuration = configuration
            };

            Add(saml);
            Add(encryption);
        }

        public void AddSaml2SecurityTokenHandler(string identifier, SecurityTokenHandlerConfiguration configuration)
        {
            var saml = new HttpSamlSecurityTokenHandler(identifier)
            {
                Configuration = configuration
            };

            Add(saml);
        }

        public void AddSimpleWebTokenHandler(string identifier, string issuer, string audience, string signingKey)
        {
            var configuration = new SecurityTokenHandlerConfiguration();

            // issuer name registry
            var registry = new WebTokenIssuerNameRegistry();
            registry.AddTrustedIssuer(issuer, issuer);
            configuration.IssuerNameRegistry = registry;

            // issuer signing key resolver
            var issuerResolver = new WebTokenIssuerTokenResolver();
            issuerResolver.AddSigningKey(issuer, signingKey);
            configuration.IssuerTokenResolver = issuerResolver;

            // audience restriction
            configuration.AudienceRestriction.AllowedAudienceUris.Add(new Uri(audience));

            var swt = new SimpleWebTokenHandler(identifier)
            {
                Configuration = configuration
            };

            Add(swt);
        }

        public ClaimsIdentity ValidateToken(string identifier, string tokenString)
        {
            var handler = this[identifier];
            if (handler == null)
            {
                throw new SecurityTokenValidationException("Unknown token identifier");
            }

            try
            {
                var token = handler.ReadToken(tokenString);
                return handler.ValidateToken(token).First();
            }
            catch
            {
                throw new SecurityTokenValidationException("Invalid security token");
            }
        }

        #region NotImplemented
        public new void AddOrReplace(SecurityTokenHandler handler)
        {
            throw new NotImplementedException();
        }

        public new ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
