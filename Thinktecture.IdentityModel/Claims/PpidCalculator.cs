/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Thinktecture.IdentityModel.Extensions;

namespace Thinktecture.IdentityModel.Claims
{
    /// <summary>
    /// Calculates PPIDs
    /// </summary>
    public static class PpidCalculator
    {
        /// <summary>
        /// Calculates the PPID for the SSL case.
        /// </summary>
        /// <param name="cardId">The card id.</param>
        /// <param name="certificate">The certificate.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns>A string containing the calculated PPID</returns>
        public static string Calculate(string cardId, X509Certificate2 certificate, string entropy)
        {
            Contract.Requires(!string.IsNullOrEmpty(cardId));
            Contract.Requires(certificate != null);
            Contract.Requires(entropy != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
            
            string id;
            string o; string l; string s; string c; string cn;
            ExtractValues(certificate, out o, out l, out s, out c, out cn);

            // organization is provided
            if (!string.IsNullOrEmpty(o))
            {
                string format = "|O=\"{0}\"|L=\"{1}\"|S=\"{2}\"|C=\"{3}\"|";
                id = string.Format(format, o, l, s, c);
            }
            // common name is provided
            else if (!string.IsNullOrEmpty(cn))
            {
                id = string.Format("|CN=\"{0}\"|", cn);
            }
            // no common name
            else
            {
                var hash = ((RSACryptoServiceProvider)certificate.PublicKey.Key).GetKeyHash();
                return Calculate(cardId, hash, entropy);
            }

            byte[] idBytes = Encoding.Unicode.GetBytes(id);
            byte[] ppidSeed = SHA256.Create().ComputeHash(idBytes);

            return Calculate(cardId, ppidSeed, entropy);
        }

        /// <summary>
        /// Calculates the PPID for the No-SSL case.
        /// </summary>
        /// <param name="cardId">The card id.</param>
        /// <param name="dns">The DNS.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns>A string containing the calculated PPID</returns>
        public static string Calculate(string cardId, string dns, string entropy)
        {
            Contract.Requires(!string.IsNullOrEmpty(cardId));
            //Contract.Requires(Uri.CheckHostName(dns) == UriHostNameType.Dns);
            Contract.Requires(entropy != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));


            if (string.IsNullOrEmpty("dns"))
            {
                throw new ArgumentNullException("dns");
            }

            byte[] dnsBytes = Encoding.Unicode.GetBytes(dns.ToLower());
            byte[] ppidSeed = SHA256.Create().ComputeHash(dnsBytes);

            return Calculate(cardId, ppidSeed, entropy);
        }

        /// <summary>
        /// Calculates the PPID using the client pseudonym.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="clientPseudonym">The client pseudonym.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns>A string containing the calculated PPID</returns>
        public static string CalculatePseudonym(string userId, string clientPseudonym, string entropy)
        {
            Contract.Requires(!string.IsNullOrEmpty(userId));
            Contract.Requires(!string.IsNullOrEmpty(clientPseudonym));
            Contract.Requires(!string.IsNullOrEmpty(entropy));
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));


            byte[] bytes = Encoding.Unicode.GetBytes(userId + clientPseudonym + entropy);
            byte[] hash = SHA256.Create().ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Calculates the PPID for a given CardId, PPID seed and entropy.
        /// </summary>
        /// <param name="cardId">The card id.</param>
        /// <param name="ppidSeed">The ppid seed.</param>
        /// <param name="entropy">The entropy.</param>
        /// <returns>A string containing the calculated PPID</returns>
        private static string Calculate(string cardId, byte[] ppidSeed, string entropy)
        {
            Contract.Requires(!string.IsNullOrEmpty(cardId));
            Contract.Requires(ppidSeed != null);
            Contract.Requires(ppidSeed.Length > 0);
            Contract.Requires(entropy != null);
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
            

            // card id is required to calculate PPID
            if (string.IsNullOrEmpty(cardId))
            {
                // no PPID to calculate
                throw new ArgumentNullException("cardId");
            }

            byte[] cardIdBytes = Encoding.Unicode.GetBytes(cardId);
            byte[] canonicalCardId = SHA256.Create().ComputeHash(cardIdBytes);

            byte[] entropyBytes = Encoding.Unicode.GetBytes(entropy);
            byte[] ppidBytes = new byte[ppidSeed.Length + canonicalCardId.Length + entropyBytes.Length];
            ppidSeed.CopyTo(ppidBytes, 0);
            canonicalCardId.CopyTo(ppidBytes, ppidSeed.Length);
            entropyBytes.CopyTo(ppidBytes, ppidSeed.Length + canonicalCardId.Length);

            byte[] ppidHash = SHA256.Create().ComputeHash(ppidBytes);

            return Convert.ToBase64String(ppidHash);
        }

        /// <summary>
        /// Extracts the O, L, S, C, CN values from a subject name
        /// </summary>
        /// <param name="certificate">The certificate.</param>
        /// <param name="o">The o.</param>
        /// <param name="l">The l.</param>
        /// <param name="s">The s.</param>
        /// <param name="c">The c.</param>
        /// <param name="cn">The cn.</param>
        private static void ExtractValues(X509Certificate2 certificate, out string o, out string l, out string s, out string c, out string cn)
        {
            Contract.Requires(certificate != null);
            Contract.Ensures(Contract.ValueAtReturn(out o) != null);
            Contract.Ensures(Contract.ValueAtReturn(out l) != null);
            Contract.Ensures(Contract.ValueAtReturn(out s) != null);
            Contract.Ensures(Contract.ValueAtReturn(out c) != null);
            Contract.Ensures(Contract.ValueAtReturn(out cn) != null);


            string subject = certificate.SubjectName.Name;

            string[] segments = subject.Split(',');
            Contract.Assert(Contract.ForAll(segments, seg => seg != null));
            Contract.Assert(segments != null);

            for (int i = 0; i < segments.Length; i++)
            {
                segments[i] = segments[i].Trim();
            }

            o = GetValueFromSubject("O", segments);
            l = GetValueFromSubject("L", segments);
            s = GetValueFromSubject("S", segments);
            c = GetValueFromSubject("C", segments);
            cn = GetValueFromSubject("CN", segments);

            return;
        }

        /// <summary>
        /// Gets a value from a splitted subject.
        /// </summary>
        /// <param name="part">The part.</param>
        /// <param name="segments">The subject segments.</param>
        /// <returns></returns>
        private static string GetValueFromSubject(string part, string[] segments)
        {
            Contract.Requires(!string.IsNullOrEmpty(part));
            Contract.Requires(segments != null);
            Contract.Requires(Contract.ForAll(segments, s => s != null));
            Contract.Ensures(Contract.Result<string>() != null);


            return (from s in segments
                    where s.StartsWith(String.Format("{0}=", part), StringComparison.OrdinalIgnoreCase)
                    select s.Substring(part.Length + 1))
                    .FirstOrDefault() ?? string.Empty;
        }
    }
}
