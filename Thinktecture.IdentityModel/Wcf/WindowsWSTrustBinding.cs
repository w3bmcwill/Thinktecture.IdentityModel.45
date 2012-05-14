using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel45.Bindings
{
    public class WindowsWSTrustBinding : WSTrustBindingBase
    {
        // Methods
        public WindowsWSTrustBinding()
            : this(SecurityMode.Message)
        {
        }

        public WindowsWSTrustBinding(SecurityMode securityMode)
            : base(securityMode)
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
                return SecurityBindingElement.CreateSspiNegotiationBindingElement(true);
            }
            if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
            {
                return SecurityBindingElement.CreateSspiNegotiationOverTransportBindingElement(true);
            }
            return null;
        }
    }
}