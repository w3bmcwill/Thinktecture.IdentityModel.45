using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thinktecture.IdentityModel.Http.Cors
{
    public interface IHttpRequestWrapper
    {
        string Resource { get; }
        IDictionary<string, object> Properties { get; }
        string Method { get; }
        string GetHeader(string name);
    }
}
