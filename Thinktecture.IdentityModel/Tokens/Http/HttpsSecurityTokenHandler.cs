using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    class HttpsSecurityTokenHandler : X509SecurityTokenHandler
    {
        public HttpsSecurityTokenHandler()
            : base(X509CertificateValidator.None)
        {
            Configuration = new SecurityTokenHandlerConfiguration
            {
                IssuerNameRegistry = new HttpsIssuerNameRegistry()
            };
        }
    }
}
