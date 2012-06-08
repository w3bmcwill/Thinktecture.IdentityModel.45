using System;
using System.Net.Http;
using Thinktecture.Samples;
using Thinktecture.IdentityModel.Extensions;
using Resources;

namespace AccessKeyClient
{
    class Program
    {
        static Uri _baseAddress = new Uri(Constants.ServiceBaseAddressWebHost);

        static void Main(string[] args)
        {
            while (true)
            {
                Helper.Timer(() =>
                {
                    "Calling Service\n".ConsoleYellow();

                    var client = new HttpClient { BaseAddress = _baseAddress };

                    var response = client.GetAsync("identity?key=accesskey123").Result;
                    response.EnsureSuccessStatusCode();

                    var identity = response.Content.ReadAsAsync<Identity>().Result;
                    identity.ShowConsole();
                });

                Console.ReadLine();
            }
        }
    }
}
