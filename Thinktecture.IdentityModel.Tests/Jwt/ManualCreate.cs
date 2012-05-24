using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Tests.Helper;
using Thinktecture.IdentityModel.Tokens;

namespace Thinktecture.IdentityModel.Tests.Jwt
{
    [TestClass]
    public class ManualCreate
    {
        [TestMethod]
        public void ManualWriteNoSig()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.None
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();

            var token = handler.WriteToken(jwt);
            
            Trace.WriteLine(token);

            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));

            var parts = token.Split('.');
            Assert.IsTrue(parts.Length == 2, "JWT should have excactly 2 parts");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ManualWriteHmacSha256MissingSigningCredentials()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA256
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();

            var token = handler.WriteToken(jwt);
        }

        [TestMethod]
        public void ManualWriteHmacSha256ValidSigningCredentials()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA256,
                    SigningCredentials = new HmacSigningCredentials(SymmetricKeyGenerator.Create(32))
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.WriteToken(jwt);
            Trace.WriteLine(token);

            // token should not be empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));

            // token with signature needs to be 3 parts
            var parts = token.Split('.');
            Assert.IsTrue(parts.Length == 3, "JWT should have excactly 3 parts");

            // signature must be 256 bits
            var sig = Base64Url.Decode(parts[2]);
            Assert.IsTrue(sig.Length == 32, "Signature is not 32 bits");
        }

        [TestMethod]
        public void ManualWriteHmacSha384ValidSigningCredentials()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA384,
                    SigningCredentials = new HmacSigningCredentials(SymmetricKeyGenerator.Create(48))
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.WriteToken(jwt);
            Trace.WriteLine(token);

            // token should not be empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));

            // token with signature needs to be 3 parts
            var parts = token.Split('.');
            Assert.IsTrue(parts.Length == 3, "JWT should have excactly 3 parts");

            // signature must be 384 bits
            var sig = Base64Url.Decode(parts[2]);
            Assert.IsTrue(sig.Length == 48, "Signature is not 48 bits");
        }

        [TestMethod]
        public void ManualWriteHmacSha512ValidSigningCredentials()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA512,
                    SigningCredentials = new HmacSigningCredentials(SymmetricKeyGenerator.Create(64))
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.WriteToken(jwt);
            Trace.WriteLine(token);

            // token should not be empty
            Assert.IsTrue(!string.IsNullOrWhiteSpace(token));

            // token with signature needs to be 3 parts
            var parts = token.Split('.');
            Assert.IsTrue(parts.Length == 3, "JWT should have excactly 3 parts");

            // signature must be 512 bits
            var sig = Base64Url.Decode(parts[2]);
            Assert.IsTrue(sig.Length == 64, "Signature is not 64 bits");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ManualWriteHmacSha256KeySizeMismatch()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA256,
                    SigningCredentials = new HmacSigningCredentials(SymmetricKeyGenerator.Create(48))
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.WriteToken(jwt);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ManualWriteUnsupportedSignatureAlgorithm()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = "unsupported",
                    SigningCredentials = new HmacSigningCredentials(SymmetricKeyGenerator.Create(48))
                },

                Audience = new Uri("http://foo.com"),
                Issuer = "dominick",
                ExpirationTime = 500000,

                Claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, "dominick"),
                    new Claim(ClaimTypes.Email, "dominick.baier@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();
            var token = handler.WriteToken(jwt);
        }
    }
}
