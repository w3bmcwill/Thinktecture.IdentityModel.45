using System.Security.Claims;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public interface IAuthorizationManager
    {
        bool CheckAccess(HttpActionContext context);
    }
}
