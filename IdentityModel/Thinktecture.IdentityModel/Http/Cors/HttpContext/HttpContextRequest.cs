/*
 * Copyright (c) Dominick Baier & Brock Allen.  All rights reserved.
 * see license.txt
 */

using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Thinktecture.IdentityModel.Http.Cors.HttpContext
{
    class HttpContextRequest : IHttpRequestWrapper
    {
        HttpRequest request;
        public HttpContextRequest(HttpRequest request)
        {
            this.request = request;
        }

        public string Resource
        {
            get { return request.Url.AbsolutePath; }
        }

        public IDictionary<string, object> Properties
        {
            get { return this.request.RequestContext.RouteData.Values; }
        }

        public string Method
        {
            get { return this.request.HttpMethod; }
        }

        public string GetHeader(string name)
        {
            if (request.Headers.AllKeys.Contains(name))
            {
                return request.Headers[name];
            }
            return null;
        }
    }
}
