using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;
using Resources.Security;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Resources
{
    [ApiAuthorize(typeof(ConsultantsAuthorizationManager))]
    public class ConsultantsController : ApiController
    {
        IConsultantsRepository _repository;

        public ConsultantsController()
        {
            _repository = new InMemoryConsultantsRepository();
        }

        [AllowAnonymous]
        public IQueryable<Consultant> Get()
        {
            return _repository.GetAll().AsQueryable();
        }

        [AllowAnonymous]
        public Consultant Get(int id)
        {
            var consultant = _repository.GetAll().FirstOrDefault(c => c.ID == id);

            if (consultant == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            return consultant;
        }

        public HttpResponseMessage Post(Consultant consultant)
        {
            if (!ModelState.IsValid)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }

            consultant.Owner = ClaimsPrincipal.Current.Identity.Name;
            var id = _repository.Add(consultant);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = new Uri(Request.RequestUri.AbsoluteUri + "/" + id.ToString());
            
            return response;
        }

        public void Put(int id, [FromBody] Consultant consultant)
        {
            // check if consultant exists
            var oldConsultant = _repository.GetAll().FirstOrDefault(c => c.ID == id);

            if (oldConsultant == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }

            consultant.ID = id;
            consultant.Owner = Thread.CurrentPrincipal.Identity.Name;

            // check moved to authorization manager
            //if (oldConsultant.Owner != consultant.Owner)
            //{
            //    throw new SecurityException("Not authorized to change record");
            //}
            
            _repository.Update(consultant);
        }
    }
}
