/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Thinktecture.IdentityModel.Constants;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    class HttpSaml2SecurityTokenHandler : Saml2SecurityTokenHandler
    {
        private string[] _identifier = new string[] 
            { 
                "Saml2",
                TokenTypes.OasisWssSaml2TokenProfile11,
                TokenTypes.Saml2TokenProfile11
            };

        public HttpSaml2SecurityTokenHandler()
            : base()
        { }

        public HttpSaml2SecurityTokenHandler(string identifier)
            : base()
        {
            _identifier = new string[] { identifier };
        }

        public HttpSaml2SecurityTokenHandler(SamlSecurityTokenRequirement requirement, string identifier)
            : base(requirement)
        {
            _identifier = new string[] { identifier };
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            return ReadToken(new XmlTextReader(new StringReader(tokenString)));
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return _identifier;
        }
    }
}
