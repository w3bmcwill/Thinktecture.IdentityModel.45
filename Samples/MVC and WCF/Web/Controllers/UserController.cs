using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View("Identity", HttpContext.User);
        }

        public ActionResult Token()
        {
            string tokenString;
            
            try
            {
                tokenString = GetBootstrapTokenAsString(User.Identity as ClaimsIdentity);
            }
            catch (Exception ex)
            {
                return new ContentResult
                {
                    Content = ex.Message
                };
            }

            return new ContentResult
            {
                ContentType = "text/xml",
                Content = tokenString
            };
        }

        private string GetBootstrapTokenAsString(ClaimsIdentity identity)
        {
            var context = identity.BootstrapContext as BootstrapContext;

            if (context == null)
            {
                throw new InvalidOperationException("Bootstrap context is null");
            }

            if (!string.IsNullOrWhiteSpace(context.Token))
            {
                return context.Token;
            }

            var sb = new StringBuilder(128);
            context.SecurityTokenHandler.WriteToken(
                new XmlTextWriter(new StringWriter(sb)),
                context.SecurityToken);

            return sb.ToString();
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
