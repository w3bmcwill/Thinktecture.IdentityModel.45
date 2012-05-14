/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

namespace Thinktecture.IdentityModel.Extensions
{
    public static partial class XmlExtensions
    {
        /// <summary>
        /// Converts a XmlDocument to a XDocument.
        /// </summary>
        /// <param name="document">The XmlDocument.</param>
        /// <returns>A XDocument</returns>
        public static XDocument ToXDocument(this XmlDocument document)
        {
            Contract.Requires(document != null);
            Contract.Ensures(Contract.Result<XDocument>() != null);


            return new XmlConverter(document).CreateXDocument();
        }   
    }
}