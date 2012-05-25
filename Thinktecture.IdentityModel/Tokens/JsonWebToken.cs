/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Thinktecture.IdentityModel;

namespace Thinktecture.IdentityModel.Tokens
{
    public class JsonWebToken : SecurityToken
    {
        private long? _expirationTime;
        private DateTime _validTo;

        private long? _notBefore;
        private DateTime _validFrom;

        #region SecurityToken
        public override string Id
        {
            get { return JwtId; }
        }

        public override System.Collections.ObjectModel.ReadOnlyCollection<SecurityKey> SecurityKeys
        {
            get { throw new NotImplementedException(); }
        }

        public override DateTime ValidFrom
        {
            get { return _validFrom; }
        }

        public override DateTime ValidTo
        {
            get { return _validTo; }
        }
        #endregion

        public JwtHeader Header { get; set; }

        public long? ExpirationTime
        {
            get
            {
                return _expirationTime;
            }
            set
            {
                _expirationTime = value;
                _validTo = _expirationTime.Value.ToDateTimeFromEpoch();
            }
        }

        public long? NotBefore
        {
            get
            {
                return _notBefore;
            }
            set
            {
                _notBefore = value;
                _validFrom = _notBefore.Value.ToDateTimeFromEpoch();
            }
        }

        public long? IssuedAt { get; set; }

        public string Issuer { get; set; }
        public Uri Audience { get; set; }
        public string Principal { get; set; }
        public string JwtId { get; set; }

        public string UnsignedToken { get; set; }
        public string Signature { get; set; }

        public List<Claim> Claims { get; set; }

        public JsonWebToken()
        {
            Header = new JwtHeader();
            Claims = new List<Claim>();
        }
    }
}
