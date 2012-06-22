using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public class HttpAuthorizationContext : AuthorizationContext
    {
        private HttpActionContext _context;

        protected HttpActionContext ActionContext
        {
            get { return _context; }
        }

        public HttpAuthorizationContext(HttpActionContext context)
            : base(ClaimsPrincipal.Current,
                   context.ControllerContext.ControllerDescriptor.ControllerName,
                   context.Request.Method.Method)
        {
            _context = context;
        }
    }
}
