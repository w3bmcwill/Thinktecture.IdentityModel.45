using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using Resources;
using Resources.Security;

namespace WebApiSecurity
{
    public class DependencyConfig
    {
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.DependencyResolver = new SimpleContainer();
        }
    }

    // A simple implementation of IDependencyResolver, for example purposes.
    class SimpleContainer : IDependencyResolver
    {
        static readonly IConsultantsRepository respository = new InMemoryConsultantsRepository();

        public IDependencyScope BeginScope()
        {
            // This example does not support child scopes, so we simply return 'this'.
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ConsultantsAuthorization))
            {
                return new ConsultantsAuthorization(respository);
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return new List<object>();
        }

        public void Dispose()
        {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}