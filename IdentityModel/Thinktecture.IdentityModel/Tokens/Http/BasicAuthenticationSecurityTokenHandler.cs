/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    public class BasicAuthenticationSecurityTokenHandler : UserNameSecurityTokenHandler
    {
        private string[] _identifiers = new string[] { "Basic" };

        /// <summary>
        /// Callback type for validating the credential
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>True when the credential could be validated succesfully. Otherwise false.</returns>
        public delegate bool ValidateUserNameCredentialDelegate(string username, string password);

        /// <summary>
        /// Gets or sets the credential validation callback
        /// </summary>
        /// <value>
        /// The credential validation callback.
        /// </value>
        public ValidateUserNameCredentialDelegate ValidateUserNameCredential { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUserNameSecurityTokenHandler"/> class.
        /// </summary>
        public BasicAuthenticationSecurityTokenHandler()
            : base()
        { }

        public BasicAuthenticationSecurityTokenHandler(string identifier)
            : base()
        {
            _identifiers = new string[] { identifier };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpUserNameSecurityTokenHandler"/> class.
        /// </summary>
        /// <param name="validateUserNameCredential">The credential validation callback.</param>
        public BasicAuthenticationSecurityTokenHandler(ValidateUserNameCredentialDelegate validateUserNameCredential)
            : base()
        {
            if (validateUserNameCredential == null)
            {
                throw new ArgumentNullException("ValidateUserNameCredential");
            }

            ValidateUserNameCredential = validateUserNameCredential;
        }

        public BasicAuthenticationSecurityTokenHandler(ValidateUserNameCredentialDelegate validateUserNameCredential, string identifier)
            : this(validateUserNameCredential)
        {
            _identifiers = new string[] { identifier };
        }

        public override SecurityToken ReadToken(string tokenString)
        {
            var credential = DecodeBasicAuthenticationHeader(tokenString);
            return new UserNameSecurityToken(credential.Item1, credential.Item2);
        }

        /// <summary>
        /// Validates the username and password.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>An identity collection representing the identity in the token</returns>
        public override ReadOnlyCollection<ClaimsIdentity> ValidateToken(SecurityToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (base.Configuration == null)
            {
                throw new InvalidOperationException("No Configuration set");
            }

            UserNameSecurityToken unToken = token as UserNameSecurityToken;
            if (unToken == null)
            {
                throw new ArgumentException("SecurityToken is not a UserNameSecurityToken");
            }

            if (!ValidateUserNameCredentialCore(unToken.UserName, unToken.Password))
            {
                throw new SecurityTokenValidationException(unToken.UserName);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, unToken.UserName),
                new Claim(ClaimTypes.AuthenticationMethod, AuthenticationMethods.Password),
                AuthenticationInstantClaim.Now
            };

            var identity = new ClaimsIdentity(claims, "Basic");

            if (Configuration.SaveBootstrapContext)
            {
                if (this.RetainPassword)
                {
                    identity.BootstrapContext = new BootstrapContext(unToken, this);
                }
                else
                {
                    var bootstrapToken = new UserNameSecurityToken(unToken.UserName, null);
                    identity.BootstrapContext = new BootstrapContext(bootstrapToken, this);
                }
            }

            return new List<ClaimsIdentity> { identity }.AsReadOnly();
        }

        /// <summary>
        /// Gets a value indicating whether this instance can validate a token.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can validate a token; otherwise, <c>false</c>.
        /// </value>
        public override bool CanValidateToken
        {
            get
            {
                return true;
            }
        }

        public override string[] GetTokenTypeIdentifiers()
        {
            return _identifiers;
        }

        protected virtual Tuple<string, string> DecodeBasicAuthenticationHeader(string basicAuthToken)
        {
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string userPass = encoding.GetString(Convert.FromBase64String(basicAuthToken));
            int separator = userPass.IndexOf(':');

            var credential = new Tuple<string, string>(
                userPass.Substring(0, separator),
                userPass.Substring(separator + 1));

            return credential;
        }

        /// <summary>
        /// Validates the user name credential core.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        protected virtual bool ValidateUserNameCredentialCore(string userName, string password)
        {
            if (ValidateUserNameCredential == null)
            {
                throw new InvalidOperationException("ValidateUserNameCredentialDelegate not set");
            }

            return ValidateUserNameCredential(userName, password);
        }
    }
}
