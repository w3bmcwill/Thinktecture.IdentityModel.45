/*
 * Copyright (c) Dominick Baier & Brock Allen.  All rights reserved.
 * see license.txt
 */

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
