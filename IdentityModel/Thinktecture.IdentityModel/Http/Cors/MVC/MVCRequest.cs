using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Thinktecture.IdentityModel.Http.Cors.MVC
{
    class MVCRequest : IHttpRequestWrapper
    {
        HttpRequestBase request;
        public MVCRequest(HttpRequestBase request)
        {
            this.request = request;
        }

        public string Resource
        {
            get { return this.request.RequestContext.RouteData.Values["controller"] as string; }
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
            if (this.request.Headers.AllKeys.Contains(name))
            {
                return this.request.Headers[name];
            }
            return null;
        }
    }
}
