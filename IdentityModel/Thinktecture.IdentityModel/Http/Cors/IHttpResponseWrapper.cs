using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinktecture.IdentityModel.Http.Cors
{
    public interface IHttpResponseWrapper
    {
        void AddHeader(string name, string value);
    }
}
