using System;
using System.Net;
using System.Net.Http;
using Thinktecture.Samples;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.Tokens.Http;
using Resources;
using System.Net.Http.Formatting;


namespace ConsultantsConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunAs(null);
            //Console.ReadLine();
            RunAs("dominick");
            Console.ReadLine();
            //RunAs("alice");
        }

        static void RunAs(string userName)
        {
            Console.Clear();

            userName = userName ?? "anonymous";

            string.Format("Using resource as {0}\n\n", userName).ConsoleYellow();

            // set up the client
            //var client = new HttpClient { BaseAddress = new Uri(Constants.WebHostBaseAddress) };
            var client = new HttpClient { BaseAddress = new Uri(Constants.SelfHostBaseAddress) };

            if (!userName.Equals("anonymous"))
            {
                client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(userName, userName);
            }

            GetIdentity(client);

            Console.ReadLine();
            ListConsultants(client);
            var location = AddConsultant(client);

            if (!string.IsNullOrEmpty(location))
            {
                ListConsultants(client);
                UpdateConsultant(client, location);
            }
        }

        private static void UpdateConsultant(HttpClient client, string location)
        {
            "\nUpdate consultant...".ConsoleGreen();

            var newClient = new HttpClient { BaseAddress = new Uri(location) };
            if (client.DefaultRequestHeaders.Authorization != null)
            {
                newClient.DefaultRequestHeaders.Authorization = client.DefaultRequestHeaders.Authorization;
            }

            var updatedConsultant = new Consultant
            {
                Name = "Brock Allen",
                Country = "US",
                EmailAddress = "brock.allen@thinktecture.com"
            };

            var response = newClient.PutAsJsonAsync<Consultant>("", updatedConsultant).Result;
            response.EnsureSuccessStatusCode();

            ListConsultants(client);
        }

        private static void GetIdentity(HttpClient client)
        {
            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var identity = response.Content.ReadAsAsync<Identity>().Result;
            identity.ShowConsole();
        }
        
        private static HttpResponseMessage ListConsultants(HttpClient client)
        {
            // list consultants
            var response = client.GetAsync("consultants").Result;
            response.EnsureSuccessStatusCode();

            var consultants = response.Content.ReadAsAsync<Consultants>().Result;

            "\nList consultants...".ConsoleGreen();
            consultants.ForEach(c => 
                {
                    Console.WriteLine(c.Name);
                    Console.WriteLine(" {0} ({1})", c.EmailAddress, c.Country);
                    Console.WriteLine(" ({0})", c.Owner);
                    Console.WriteLine();
                });
            return response;
        }

        private static string AddConsultant(HttpClient client)
        {
            // add consultant
            var newConsultant = new Consultant
            {
                Name = "Brock Allen",
                Country = "US",
                EmailAddress = "brock.alln@thinktecture.com",
            };

            "\nAdd new consultant...".ConsoleGreen();
            var response = client.PostAsJsonAsync<Consultant>("consultants", newConsultant).Result;

            if (response.StatusCode == HttpStatusCode.Created)
            {
                Console.WriteLine("success.");
                Console.WriteLine("location: {0}", response.Headers.Location);
                return response.Headers.Location.OriginalString;
            }
            else
            {
                Console.WriteLine("failed.");
                Console.WriteLine(response.StatusCode);
                return null;
            }
        }

        
    }
}
