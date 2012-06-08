using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel;

namespace Thinktecture.IdentityModel
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public class X509CertificatesLocation
    {
        StoreLocation _location;

        public X509CertificatesLocation(StoreLocation location)
        {
            _location = location;
        }

        public X509CertificatesName My
        {
            get
            {
                return new X509CertificatesName(_location, StoreName.My);
            }
        }

        public X509CertificatesName AddressBook
        {
            get
            {
                return new X509CertificatesName(_location, StoreName.AddressBook);
            }
        }

        public X509CertificatesName TrustedPeople
        {
            get
            {
                return new X509CertificatesName(_location, StoreName.TrustedPeople);
            }
        }

        public X509CertificatesName TrustedPublisher
        {
            get
            {
                return new X509CertificatesName(_location, StoreName.TrustedPublisher);
            }
        }

        public X509CertificatesName CertificateAuthority
        {
            get
            {
                return new X509CertificatesName(_location, StoreName.CertificateAuthority);
            }
        }
    }
}
