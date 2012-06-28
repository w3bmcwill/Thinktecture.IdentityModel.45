using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CorsSampleMvc.Controllers
{
    public class Values3Controller : Controller
    {
        public ActionResult GetData()
        {
            Response.AddHeader("Foo", "foo");
            Response.AddHeader("Bar", "bar");
            return Json(new { foo = "foo" }, JsonRequestBehavior.AllowGet);
        }
    }
}
