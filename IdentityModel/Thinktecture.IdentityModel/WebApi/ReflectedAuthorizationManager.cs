using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public class ReflectedAuthorizationManager : IAuthorizationManager
    {
        public bool CheckAccess(HttpActionContext context)
        {
            var action = Format(context.Request.Method.Method);
            var controller = Format(context.ControllerContext.ControllerDescriptor.ControllerName);
            var manager = controller + "Authorization";

            var info = GetMethodInfo(action, manager);

            return true;
        }

        private MethodInfo GetMethodInfo(string action, string manager)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetExportedTypes();

                foreach (var expType in types)
                {
                    if (expType.Name == manager)
                    {
                        var s = expType;
                    }
                }
            }

            return null;
        }
        
        protected virtual string Format(string input)
        {
            // get the first letter an make it uppercase
            var f = input.Substring(0, 1).ToUpperInvariant();

            // get the rest lowercase
            var r = input.Substring(1).ToLowerInvariant();

            return f + r;
        }

    }
}
