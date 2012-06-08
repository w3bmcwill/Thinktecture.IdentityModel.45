using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Tests.Helper
{
    internal static class SymmetricKeyGenerator
    {
        public static byte[] Create(int keySize)
        {
            var bytes = new byte[keySize];
            new RNGCryptoServiceProvider().GetBytes(bytes);

            return bytes;
        }
    }
}
