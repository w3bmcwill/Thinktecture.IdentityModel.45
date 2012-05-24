using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Thinktecture.IdentityModel.Bindings
{
    public class CertificateWSTrustBinding : WSTrustBindingBase
    {
        // Methods
        public CertificateWSTrustBinding()
            : this(SecurityMode.Message)
        {
        }

        public CertificateWSTrustBinding(SecurityMode securityMode)
            : base(securityMode)
        {
        }

        protected override void ApplyTransportSecurity(HttpTransportBindingElement transport)
        {
            transport.AuthenticationScheme = AuthenticationSchemes.Anonymous;
            HttpsTransportBindingElement element = transport as HttpsTransportBindingElement;
            if (element != null)
            {
                element.RequireClientCertificate = true;
            }
        }

        protected override SecurityBindingElement CreateSecurityBindingElement()
        {
            if (SecurityMode.Message == base.SecurityMode)
            {
                return SecurityBindingElement.CreateMutualCertificateBindingElement();
            }
            if (SecurityMode.TransportWithMessageCredential == base.SecurityMode)
            {
                return SecurityBindingElement.CreateCertificateOverTransportBindingElement();
            }
            return null;
        }
    }
}
