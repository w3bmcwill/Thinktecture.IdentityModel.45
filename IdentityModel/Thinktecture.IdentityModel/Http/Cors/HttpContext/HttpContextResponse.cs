using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Thinktecture.IdentityModel.Http.Cors.HttpContext
{
    class HttpContextResponse : IHttpResponseWrapper
    {
        HttpResponse response;
        public HttpContextResponse(HttpResponse response)
        {
            this.response = response;
        }

        public void AddHeader(string name, string value)
        {
            response.AddHeader(name, value);
        }
    }
}
