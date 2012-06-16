using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Security;
using Resources;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.WSTrust;
using Thinktecture.Samples;

namespace AdfsSamlClient
{
    class Program
    {
        static Uri _baseAddress = new Uri(Constants.WebHostBaseAddress);
        //static Uri _baseAddress = new Uri(Constants.SelfHostBaseAddress);
        
        static EndpointAddress _idpEndpoint =
            new EndpointAddress("https://adfs.leastprivilege.vm/adfs/services/trust/13/windowstransport");

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
            "Requesting identity token".ConsoleYellow();

            var factory = new WSTrustChannelFactory(
                new WindowsWSTrustBinding(SecurityMode.Transport),
                _idpEndpoint);
            factory.TrustVersion = TrustVersion.WSTrust13;

            var rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,
                TokenType = TokenTypes.Saml2TokenProfile11,
                AppliesTo = new EndpointReference(Constants.Realm)
            };

            var token = factory.CreateChannel().Issue(rst) as GenericXmlSecurityToken;
            return token.TokenXml.OuterXml;
        }

        private static Identity CallService(string saml)
        {
            "Calling service".ConsoleYellow();

            var client = new HttpClient { BaseAddress = _baseAddress };
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("AdfsSaml", saml);

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<Identity>().Result;
        }
    }
}
