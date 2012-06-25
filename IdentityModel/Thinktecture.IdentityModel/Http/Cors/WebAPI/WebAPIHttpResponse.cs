using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;

namespace Thinktecture.IdentityModel.Http.Cors.WebAPI
{
    class WebAPIHttpResponse : IHttpResponseWrapper
    {
        HttpResponseMessage response;

        public WebAPIHttpResponse(HttpResponseMessage response)
        {
            this.response = response;
        }

        public void AddHeader(string name, string value)
        {
            this.response.Headers.Add(name, value);
        }
    }
}
