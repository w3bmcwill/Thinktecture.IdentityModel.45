/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Security.Claims;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.Authorization.WebApi
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
