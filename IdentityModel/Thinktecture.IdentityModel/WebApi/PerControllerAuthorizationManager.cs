using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public abstract class PerControllerAuthorizationManager : IAuthorizationManager
    {
        protected abstract bool Get(HttpActionContext context);
        protected abstract bool Put(HttpActionContext context);
        protected abstract bool Post(HttpActionContext context);
        protected abstract bool Delete(HttpActionContext context);

        public bool CheckAccess(HttpActionContext context)
        {
            try
            {
                switch (context.Request.Method.Method)
                {
                    case "GET":
                        return Get(context);
                    case "POST":
                        return Post(context);
                    case "PUT":
                        return Put(context);
                    case "DELETE":
                        return Delete(context);
                }
            }
            catch (NotImplementedException)
            {
                return false;
            }

            throw new InvalidOperationException("Method not supported by authorization manager");
        }
    }
}
