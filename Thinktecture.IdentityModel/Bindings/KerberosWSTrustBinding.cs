using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Thinktecture.IdentityModel.Bindings
{
    public class KerberosWSTrustBinding : WSTrustBindingBase
    {
        // Methods
        public KerberosWSTrustBinding()
            : this(SecurityMode.TransportWithMessageCredential)
        {
        }

        public KerberosWSTrustBinding(SecurityMode mode)
            : base(mode)
        {
        }

        protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
        {
            transport.AuthenticationScheme = AuthenticationSchemes.Negotiate;
        }

        protected override SecurityBindingElement CreateSecurityBindingElement()
        {
            if (SecurityMode.Message == base.SecurityMode)
            {
                return SecurityBindingElement.CreateKerberosBindingElement();
            }
            if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
            {
                return SecurityBindingElement.CreateKerberosOverTransportBindingElement();
            }
            return null;
        }
    }
}
