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
                Helper.Timer(() =>
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
                    Console.WriteLine(json.ToString());

                    "\n\nCalling service\n".ConsoleYellow();

                    client = new HttpClient { BaseAddress = _baseAddress };
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Session", token);

                    response = client.GetAsync("identity").Result;
                    response.EnsureSuccessStatusCode();

                    var identity = response.Content.ReadAsAsync<Identity>().Result;
                    identity.ShowConsole();

                });

                Console.ReadLine();
            }
        }
    }
}
