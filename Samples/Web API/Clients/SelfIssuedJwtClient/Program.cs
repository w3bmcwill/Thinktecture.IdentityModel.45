using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Resources;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.Samples;
using Sys = System.Security.Claims;

namespace JsonWebTokenClient
{
    class Program
    {
        static Uri _baseAddress = new Uri(Constants.ServiceBaseAddressWebHost);

        static void Main(string[] args)
        {
            var jwt = CreateJsonWebToken();

            while (true)
            {
                Helper.Timer(() =>
                {
                    "Calling Service\n".ConsoleYellow();

                    var client = new HttpClient { BaseAddress = _baseAddress };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("JWT", jwt);
                    
                    var response = client.GetAsync("identity").Result;
                    response.EnsureSuccessStatusCode();

                    var identity = response.Content.ReadAsAsync<Identity>().Result;
                    identity.ShowConsole();
                });

                Console.ReadLine();
            }
        }

        private static string CreateJsonWebToken()
        {
            var jwt = new JsonWebToken
            {
                Header = new JwtHeader
                {
                    SignatureAlgorithm = JwtConstants.SignatureAlgorithms.HMACSHA256,
                    SigningCredentials = new HmacSigningCredentials(Constants.IdSrvSymmetricSigningKey)
                },

                Issuer = "http://selfissued.test",
                Audience = new Uri(Constants.Realm),

                Claims = new List<Sys.Claim>
                {
                    new Sys.Claim(Sys.ClaimTypes.Name, "bob"),
                    new Sys.Claim(Sys.ClaimTypes.Email, "bob@thinktecture.com")
                }
            };

            var handler = new JsonWebTokenHandler();
            return handler.WriteToken(jwt);
        }
    }
}
