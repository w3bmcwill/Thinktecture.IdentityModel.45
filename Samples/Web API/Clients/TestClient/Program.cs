using System;
using System.Net.Http;
using Thinktecture.Samples;
using Thinktecture.Samples.Resources.Data;
using Thinktecture.IdentityModel.Http;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient { BaseAddress = new Uri(Constants.ServiceBaseAddressWebHost) };
            //client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue("alice", "alice");
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("invalid", "invalid");

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var text = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(text);
            Console.WriteLine();

            var identity = response.Content.ReadAsAsync<Identity>().Result;

            if (!identity.IsAuthenticated)
            {
                Console.WriteLine("anonymous client");
            }
        }
    }
}
