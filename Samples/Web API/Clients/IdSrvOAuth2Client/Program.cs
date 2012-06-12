using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Resources;
using Thinktecture.IdentityModel.Clients;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.Samples;


namespace IdSrvOAuth2Client
{
    class Program
    {
        static Uri _baseAddress = new Uri(Constants.WebHostBaseAddress);
        //static Uri _baseAddress = new Uri(Constants.SelfHostBaseAddress);

        static Uri _oauth2Address = new Uri("https://" + Constants.IdSrv + "/idsrv/issue/oauth2/");

        static void Main(string[] args)
        {
            while (true)
            {
                Identity id = null;
                Console.Clear();

                Helper.Timer(() =>
                {
                    var token = GetIdentityToken();
                    id = CallService(token);
                });

                id.ShowConsole();

                Console.ReadLine();
            }
        }

        private static string GetIdentityToken()
        {
            "Requesting token".ConsoleYellow();

            var client = new OAuth2Client(_oauth2Address);
            var response = client.RequestAccessTokenUserName("bob", "abc!123", Constants.Realm);

            return response.AccessToken;
        }

        private static Identity CallService(string swt)
        {
            "Calling service".ConsoleYellow();

            var client = new HttpClient { BaseAddress = _baseAddress };
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("IdSrv", swt);

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<Identity>().Result;
        }
    }
}
