using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Thinktecture.IdentityModel;
using Thinktecture.IdentityModel.Claims;
using Thinktecture.IdentityModel.Tokens;
using Thinktecture.IdentityModel.Tokens.Http;
using Thinktecture.Samples;
using Thinktecture.IdentityModel.WebApi;
using Resources.Security;
using Resources;

namespace SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new HttpSelfHostConfiguration(Constants.SelfHostBaseAddress);
            var a = Assembly.Load("resources");

            ConfigureApis(config);

            config.Routes.MapHttpRoute(
                "API Default",
                "{controller}/{id}",
                new { id = RouteParameter.Optional });

            var server = new HttpSelfHostServer(config);

            server.OpenAsync().Wait();

            Console.WriteLine("Server is running.");
            Console.ReadLine();

        }

        private static void ConfigureApis(HttpSelfHostConfiguration configuration)
        {
            // authentication
            var authConfig = AuthenticationConfig.CreateConfiguration();
            authConfig.ClaimsAuthenticationManager = new ConsultantsClaimsTransformer();
            configuration.MessageHandlers.Add(new AuthenticationHandler(authConfig));

            // authorization
            configuration.SetAuthorizationManager(new GlobalAuthorization(DefaultPolicy.Deny));

            // dependency resolver for authorization manager
            configuration.DependencyResolver = new AuthorizationDependencyResolver();
        }
    }
}
