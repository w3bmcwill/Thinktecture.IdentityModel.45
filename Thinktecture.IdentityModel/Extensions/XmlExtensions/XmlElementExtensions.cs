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
        /// Converts a XmlElement to a XElement.
        /// </summary>
        /// <param name="element">The XmlElement.</param>
        /// <returns>A XElement</returns>
        public static XElement ToXElement(this XmlElement element)
        {
            Contract.Requires(element != null);
            Contract.Ensures(Contract.Result<XElement>() != null);


            return new XmlConverter(element).CreateXElement();
        }        
    }
}