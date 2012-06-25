/*
 * Copyright (c) Dominick Baier & Brock Allen.  All rights reserved.
 * see license.txt
 */

using System;
using System.Web;

namespace Thinktecture.IdentityModel.Http.Cors.HttpContext
{
    public class CorsHttpModule : IHttpModule
    {
        public void Init(HttpApplication app)
        {
            app.BeginRequest += app_BeginRequest;
        }

        void app_BeginRequest(object sender, EventArgs e)
        {
            var ctx = System.Web.HttpContext.Current;

            var httpRequest = new HttpContextRequest(ctx.Request);
            var accessRequest = new CorsAccessRequest(httpRequest);
            var accessResponse = HttpContextConfiguration.Configuration.Engine.CheckAccess(accessRequest);
            if (accessResponse != null)
            {
                var response = ctx.Response;
                var httpResponse = new HttpContextResponse(response);
                accessResponse.WriteResponse(httpResponse);
            }

            if (accessRequest.IsCorsPreflight)
            {
                ctx.Response.StatusCode = 200;
                ctx.Response.End();
            }
        }
        
        public void Dispose()
        {
        }
    }
}
