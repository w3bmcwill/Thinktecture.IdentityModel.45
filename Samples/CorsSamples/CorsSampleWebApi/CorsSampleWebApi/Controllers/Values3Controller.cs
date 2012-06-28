using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CorsSampleWebApi.Models;

namespace WebApiSample.Controllers
{
    public class Values3Controller : ApiController
    {
        // GET api/values
        public HttpResponseMessage Get()
        {
            var resp = Request.CreateResponse<IEnumerable<SomeValue>>(HttpStatusCode.OK, new SomeValue[] { new SomeValue { Value = "value1" }, new SomeValue { Value = "value2" } });
            resp.Headers.Add("Foo", "foo");
            resp.Headers.Add("Bar", "bar");
            return resp;
        }

        // GET api/values/5
        public SomeValue Get(int id)
        {
            return new SomeValue { Value = "value" };
        }

        // POST api/values
        public void Post(SomeValue value)
        {
        }

        // PUT api/values/5
        public void Put(int id, SomeValue value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}