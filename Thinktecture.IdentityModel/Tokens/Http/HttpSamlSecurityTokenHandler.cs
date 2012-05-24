using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    class HttpSamlSecurityTokenHandler : SamlSecurityTokenHandler
    {
        private string[] _identifier = new string[] { "Saml" };

        public HttpSamlSecurityTokenHandler()
            : base()
        { }

        public HttpSamlSecurityTokenHandler(string identifier)
            : base()
        {
            _identifier = new string[] { identifier };
        }

        public HttpSamlSecurityTokenHandler(SamlSecurityTokenRequirement requirement)
            : base(requirement)
        { }

        public HttpSamlSecurityTokenHandler(SamlSecurityTokenRequirement requirement, string identifier)
            : base(requirement)
        {
            _identifier = new string[] { identifier };
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            return ContainingCollection.ReadToken(new XmlTextReader(new StringReader(tokenString)));
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return _identifier;
        }
    }
}
