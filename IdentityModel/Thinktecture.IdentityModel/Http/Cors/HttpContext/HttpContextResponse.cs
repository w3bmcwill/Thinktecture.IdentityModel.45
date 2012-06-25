/*
 * Copyright (c) Dominick Baier & Brock Allen.  All rights reserved.
 * see license.txt
 */

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
