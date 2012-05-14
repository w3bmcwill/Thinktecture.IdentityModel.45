/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;

namespace Thinktecture.IdentityModel
{
    /// <summary>
    /// Helper class for hex-encoding related work
    /// </summary>
    public static class HexEncoder
    {
        /// <summary>
        /// Converts hex to digit.
        /// </summary>
        /// <param name="val">The hex value</param>
        /// <returns></returns>
        public static int ConvertHexDigit(char val)
        {
            if ((val <= '9') && (val >= '0'))
            {
                return (val - '0');
            }
            if ((val >= 'a') && (val <= 'f'))
            {
                return ((val - 'a') + 10);
            }
            if ((val < 'A') || (val > 'F'))
            {
                throw new ArgumentException("ArgumentOutOfRange_Index");
            }
            return ((val - 'A') + 10);
        }

        /// <summary>
        /// Decodes a hex string.
        /// </summary>
        /// <param name="hexString">The hex string.</param>
        /// <returns></returns>
        public static byte[] DecodeHexString(string hexString)
        {
            byte[] buffer;
            if (hexString == null)
            {
                throw new ArgumentNullException("hexString");
            }
            bool flag = false;
            int num = 0;
            int length = hexString.Length;
            if (((length >= 2) && (hexString[0] == '0')) && ((hexString[1] == 'x') || (hexString[1] == 'X')))
            {
                length = hexString.Length - 2;
                num = 2;
            }
            if (((length % 2) != 0) && ((length % 3) != 2))
            {
                throw new ArgumentException("Argument_InvalidHexFormat");
            }
            if ((length >= 3) && (hexString[num + 2] == ' '))
            {
                flag = true;
                buffer = new byte[(length / 3) + 1];
            }
            else
            {
                buffer = new byte[length / 2];
            }
            for (int i = 0; num < hexString.Length; i++)
            {
                int num4 = ConvertHexDigit(hexString[num]);
                int num3 = ConvertHexDigit(hexString[num + 1]);
                buffer[i] = (byte)(num3 | (num4 << 4));
                if (flag)
                {
                    num++;
                }
                num += 2;
            }
            return buffer;
        }

        /// <summary>
        /// Encodes a hex string.
        /// </summary>
        /// <param name="sArray">The s array.</param>
        /// <returns></returns>
        public static string EncodeHexString(byte[] sArray)
        {
            string str = null;
            if (sArray == null)
            {
                return str;
            }
            char[] chArray = new char[sArray.Length * 2];
            int index = 0;
            int num3 = 0;
            while (index < sArray.Length)
            {
                int num = (sArray[index] & 240) >> 4;
                chArray[num3++] = HexDigit(num);
                num = sArray[index] & 15;
                chArray[num3++] = HexDigit(num);
                index++;
            }
            return new string(chArray);
        }

        internal static string EncodeHexStringFromInt(byte[] sArray)
        {
            string str = null;
            if (sArray == null)
            {
                return str;
            }
            char[] chArray = new char[sArray.Length * 2];
            int length = sArray.Length;
            int num3 = 0;
            while (length-- > 0)
            {
                int num = (sArray[length] & 240) >> 4;
                chArray[num3++] = HexDigit(num);
                num = sArray[length] & 15;
                chArray[num3++] = HexDigit(num);
            }
            return new string(chArray);
        }

        private static char HexDigit(int num)
        {
            return ((num < 10) ? ((char)(num + 48)) : ((char)(num + 55)));
        }
    }
}
