using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View("Identity", HttpContext.User);
        }

        public ActionResult Signout()
        {
            var fam = FederatedAuthentication.WSFederationAuthenticationModule;

            // clear local cookie
            fam.SignOut(false);

            // initiate a federated sign out request to the sts.
            var signOutRequest = new SignOutRequestMessage(new Uri(fam.Issuer), fam.Realm);
            signOutRequest.Reply = fam.Reply;

            return new RedirectResult(signOutRequest.WriteQueryString());
        }
    }
}
