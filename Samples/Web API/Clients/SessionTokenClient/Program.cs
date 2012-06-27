using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Resources;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.Samples;

namespace SessionTokenClient
{
    class Program
    {
        static Uri _baseAddress = new Uri(Constants.WebHostBaseAddress);
        //static Uri _baseAddress = new Uri(Constants.SelfHostBaseAddress);

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();

                Helper.Timer(() =>
                {
                    var token = RequestSessionToken();
                    CallService(token);
                });

                Console.ReadLine();
            }
        }

        private static string RequestSessionToken()
        {
            "Requesting session token\n".ConsoleYellow();

            var client = new HttpClient { BaseAddress = _baseAddress };
            client.DefaultRequestHeaders.Authorization =
                new BasicAuthenticationHeaderValue("alice", "alice");

            var response = client.GetAsync("token").Result;
            response.EnsureSuccessStatusCode();

            var tokenResponse = response.Content.ReadAsStringAsync().Result;
            var json = JObject.Parse(tokenResponse);
            var token = json["access_token"].ToString();
            var expiration = long.Parse(json["expires_in"].ToString()).ToDateTimeFromEpoch();

            "\nSession Token:".ConsoleRed();
            Console.WriteLine(json.ToString());

            "\nExpiration Time:".ConsoleRed();
            Console.WriteLine(expiration);

            return token;
        }

        private static void CallService(string token)
        {
            "\n\nCalling service\n".ConsoleYellow();

            var client = new HttpClient { BaseAddress = _baseAddress };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Session", token);

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var identity = response.Content.ReadAsAsync<Identity>().Result;
            identity.ShowConsole();
        }
    }
}
