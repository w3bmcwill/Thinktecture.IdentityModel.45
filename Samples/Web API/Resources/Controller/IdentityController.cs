using System.Web.Http;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel.Http;

namespace Resources
{
    [Authorize]
    public class IdentityController : ApiController
    {
        public Identity Get()
        {
            return new Identity(User.Identity);
        }
    }
}
