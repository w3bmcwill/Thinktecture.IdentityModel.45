using System.Web.Http;
using System.Web.Http.Controllers;
using Thinktecture.IdentityModel.Http;

namespace Resources
{
    [Authorize]
    [HttpControllerConfiguration(HttpActionSelector = typeof(CorsActionSelector))] 
    public class IdentityController : ApiController
    {
        [EnableCors]
        public Identity Get()
        {
            return new Identity(User.Identity);
        }
    }
}
