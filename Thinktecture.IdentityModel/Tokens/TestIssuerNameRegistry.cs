/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.IdentityModel.Tokens;

namespace Thinktecture.IdentityModel.Tokens
{
    /// <summary>
    /// Simple implementation of an issuer registy that returns the certificate issuer name or public key hash as an issuer
    /// </summary>
    public class TestIssuerNameRegistry : IssuerNameRegistry
    {
        /// <summary>
        /// Gets the name of the issuer.
        /// </summary>
        /// <param name="securityToken">The security token.</param>
        /// <returns></returns>
        public override string GetIssuerName(SecurityToken securityToken)
        {
            if (securityToken == null)
            {
                throw new ArgumentNullException("securityToken");
            }

            X509SecurityToken token = securityToken as X509SecurityToken;
            if (token != null)
            {
                return token.Certificate.Thumbprint;
            }
            
            RsaSecurityToken token2 = securityToken as RsaSecurityToken;
            if (token2 == null)
            {
                throw new SecurityTokenException(securityToken.GetType().FullName);
            }

            return token2.Rsa.ToXmlString(false);
        }
    }
}
