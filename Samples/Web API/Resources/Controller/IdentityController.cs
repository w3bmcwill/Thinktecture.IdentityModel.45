using System.Web.Http;

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
