/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.IdentityModel.Services;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace Thinktecture.IdentityModel.Web
{
    public class ProtectedCookie
    {
        private List<CookieTransform> _transforms;
        private ChunkedCookieHandler _handler = new ChunkedCookieHandler();

        // DPAPI protection (single server)
        public ProtectedCookie()
        {
            _transforms = new List<CookieTransform>
            { 
                new DeflateCookieTransform(), 
                new ProtectedDataCookieTransform() 
            };
        }

        // RSA protection (load balanced)
        public ProtectedCookie(X509Certificate2 protectionCertificate)
        {
            _transforms = new List<CookieTransform>
            { 
                new DeflateCookieTransform(), 
                new RsaSignatureCookieTransform(protectionCertificate),
                new RsaEncryptionCookieTransform(protectionCertificate)
            };
        }

        // custom transform pipeline
        public ProtectedCookie(List<CookieTransform> transforms)
        {
            _transforms = transforms;
        }

        public void Write(string name, string value, DateTime expirationTime)
        {
            byte[] encodedBytes = EncodeCookieValue(value);

            _handler.Write(encodedBytes, name, expirationTime);
        }

        public void Write(string name, string value, DateTime expirationTime, string domain, string path)
        {
            byte[] encodedBytes = EncodeCookieValue(value);

            _handler.Write(encodedBytes,
                           name,
                           path,
                           domain,
                           expirationTime,
                           true,
                           true,
                           HttpContext.Current);
        }

        public string Read(string name)
        {
            var bytes = _handler.Read(name);

            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            return DecodeCookieValue(bytes);
        }

        public void Delete(string name)
        {
            _handler.Delete(name);
        }

        protected virtual byte[] EncodeCookieValue(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            byte[] buffer = bytes;

            foreach (var transform in _transforms)
            {
                buffer = transform.Encode(buffer);
            }

            return buffer;
        }

        protected virtual string DecodeCookieValue(byte[] bytes)
        {
            var buffer = bytes;

            for (int i = _transforms.Count; i > 0; i--)
            {
                buffer = _transforms[i-1].Decode(buffer);
            }

            return Encoding.UTF8.GetString(buffer);
        }
    }
}
