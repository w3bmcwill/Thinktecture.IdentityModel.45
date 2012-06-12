using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Security;
using Resources;
using Thinktecture.IdentityModel.Clients;
using Thinktecture.IdentityModel.Constants;
using Thinktecture.IdentityModel.Extensions;
using Thinktecture.IdentityModel.WSTrust;
using Thinktecture.Samples;

namespace AcsSamlToSwtClient
{
    class Program
    {
        static EndpointAddress _idpEndpoint =
            new EndpointAddress("https://" + Constants.IdSrv + "/idsrv/issue/wstrust/mixed/username");

        static Uri _acsBaseAddress = new Uri("https://" + Constants.ACS + "/");

        static Uri _acsWrapEndpoint = new Uri("https://" + Constants.ACS + "/WRAPv0.9");
        static Uri _acsOAuth2Endpoint = new Uri("https://" + Constants.ACS + "/v2/OAuth2-13");

        static Uri _baseAddress = new Uri(Constants.WebHostBaseAddress);
        //static Uri _baseAddress = new Uri(Constants.SelfHostBaseAddress);

        static void Main(string[] args)
        {
            while (true)
            {
                Identity id = null;
                Console.Clear();

                Helper.Timer(() =>
                {
                    var identityToken = GetIdentityToken();
                    var serviceToken = GetServiceTokenOAuth2(identityToken);

                    id = CallService(serviceToken);
                });

                id.ShowConsole();

                Console.ReadLine();
            }
        }

        private static string GetIdentityToken()
        {
            "Requesting identity token".ConsoleYellow();

            var factory = new WSTrustChannelFactory(
                new UserNameWSTrustBinding(SecurityMode.TransportWithMessageCredential),
                _idpEndpoint);
            factory.TrustVersion = TrustVersion.WSTrust13;

            factory.Credentials.UserName.UserName = "bob"; 
            factory.Credentials.UserName.Password = "abc!123";

            var rst = new RequestSecurityToken
            {
                RequestType = RequestTypes.Issue,
                KeyType = KeyTypes.Bearer,
                AppliesTo = new EndpointReference(_acsBaseAddress.AbsoluteUri)
            };

            var token = factory.CreateChannel().Issue(rst) as GenericXmlSecurityToken;

            return token.TokenXml.OuterXml;
        }

        private static string GetServiceTokenOAuth2(string samlToken)
        {
            "Converting token from SAML to SWT".ConsoleYellow();

            var client = new OAuth2Client(_acsOAuth2Endpoint);
            return client.RequestAccessTokenAssertion(
                samlToken, 
                TokenTypes.Saml2TokenProfile11, 
                Constants.Realm).AccessToken;
        }

        private static Identity CallService(string swt)
        {
            "Calling service".ConsoleYellow();

            var client = new HttpClient { BaseAddress = _baseAddress };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ACS", swt);

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsAsync<Identity>().Result;
        }
    }
}
