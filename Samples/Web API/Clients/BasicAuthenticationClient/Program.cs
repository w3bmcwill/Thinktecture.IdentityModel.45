using System;
using System.Net.Http;
using Resources;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.Samples;

namespace BasicAuthenticationClient
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
                    "Calling Service\n".ConsoleYellow();

                    var client = new HttpClient { BaseAddress = _baseAddress };
                    client.DefaultRequestHeaders.Authorization = 
                        new BasicAuthenticationHeaderValue("alice", "alice");

                    var response = client.GetAsync("identity").Result;
                    response.EnsureSuccessStatusCode();

                    var identity = response.Content.ReadAsAsync<Identity>().Result;
                    identity.ShowConsole();
                });

                Console.ReadLine();
            }
        }
    }
}
