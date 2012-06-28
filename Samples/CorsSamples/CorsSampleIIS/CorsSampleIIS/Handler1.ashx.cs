using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CorsSampleIIS
{
    /// <summary>
    /// Summary description for Handler1
    /// </summary>
    public class Handler1 : IHttpHandler
    {
        public void ProcessRequest(HttpContext ctx)
        {
            ctx.Response.AddHeader("Foo", "foo");
            ctx.Response.AddHeader("Bar", "bar");
            ctx.Response.ContentType = "text/plain";
            ctx.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}