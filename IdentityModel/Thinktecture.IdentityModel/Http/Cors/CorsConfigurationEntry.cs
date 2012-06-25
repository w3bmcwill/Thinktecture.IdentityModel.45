using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Http.Cors
{
    public class CorsConfigurationEntry : CorsConfigurationAllowProperties
    {
        public bool AllResources { get; set; }
        public string Resource { get; set; }

        public string Origin { get; set; }
    }
}
