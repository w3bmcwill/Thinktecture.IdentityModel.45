using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Resources.Security
{
    // A simple implementation of IDependencyResolver, for example purposes.
    public class AuthorizationDependencyResolver : IDependencyResolver
    {
        static readonly IConsultantsRepository respository = new InMemoryConsultantsRepository();

        public IDependencyScope BeginScope()
        {
            // This example does not support child scopes, so we simply return 'this'.
            return this;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(ConsultantsAuthorizationManager))
            {
                return new ConsultantsAuthorizationManager(respository);
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
