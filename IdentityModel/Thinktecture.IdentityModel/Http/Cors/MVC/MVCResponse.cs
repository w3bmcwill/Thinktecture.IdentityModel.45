using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Thinktecture.IdentityModel.Http.Cors.MVC
{
    class MVCResponse : IHttpResponseWrapper
    {
        HttpResponseBase response;
        public MVCResponse(HttpResponseBase response)
        {
            this.response = response;

        }
        public void AddHeader(string name, string value)
        {
            response.AddHeader(name, value);
        }
    }
}
