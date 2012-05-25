/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Thinktecture.IdentityModel.Tokens
{
    public class SimpleSecurityTokenHandler : SecurityTokenHandler
    {
        private string[] _identifier;
        public delegate ClaimsIdentity ValidateTokenDelegate(string tokenString);

        public ValidateTokenDelegate Validator { get; set; }

        public SimpleSecurityTokenHandler(string identifier)
            : this(identifier, null)
        { }

        public SimpleSecurityTokenHandler(string identifier, ValidateTokenDelegate validator)
        {
            _identifier = new string[] { identifier };
            Validator = validator;
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            return new SimpleSecurityToken(tokenString);
        }

        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            var simpleToken = token as SimpleSecurityToken;
            if (simpleToken == null)
            {
                throw new ArgumentException("SecurityToken is not a SimpleSecurityToken");
            }

            var identity = Validator(simpleToken.Value);

            if (identity != null)
            {
                if (Configuration != null && Configuration.SaveBootstrapContext)
                {
                    identity.BootstrapContext = new BootstrapContext(simpleToken, this);
                }

                return new List<ClaimsIdentity> { identity }.AsReadOnly();
            }
            else
            {
                throw new SecurityTokenValidationException("No identity");
            }
        }

        public override string WriteToken(SecurityToken token)
        {
            var simpleToken = token as SimpleSecurityToken;
            if (simpleToken == null)
            {
                throw new ArgumentException("SecurityToken is not a SimpleSecurityToken");
            }

            return simpleToken.Value;
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return _identifier;
        }

        public override Type TokenType
        {
            get { return typeof(SimpleSecurityToken); }
        }
    }
}
