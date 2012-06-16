using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Tokens.Http
{
    public class TokenServiceConfiguration
    {
        public TimeSpan DefaultTokenLifetime { get; set; }
        public string EndpointAddress { get; set; }
        public SecurityTokenHandler SecurityTokenHandler { get; set; }

        public TokenServiceConfiguration()
        {
            DefaultTokenLifetime = TimeSpan.FromHours(10);
            EndpointAddress = "/token";
            SecurityTokenHandler = new JsonWebTokenHandler();
        }
    }
}
