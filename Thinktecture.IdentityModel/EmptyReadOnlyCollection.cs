/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * 
 * This code is licensed under the Microsoft Permissive License (Ms-PL)
 * 
 * SEE: http://www.microsoft.com/resources/sharedsource/licensingbasics/permissivelicense.mspx
 * 
 */

using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Thinktecture.IdentityModel
{
    /// <summary>
    /// Represents an empty ReadOnlyCollection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class EmptyReadOnlyCollection<T>
    {
        /// <summary>
        /// Returns a singleton instance of the EmptyReadOnlyCollection
        /// </summary>
        public static ReadOnlyCollection<T> Instance;

        static EmptyReadOnlyCollection()
        {
            EmptyReadOnlyCollection<T>.Instance = new List<T>().AsReadOnly();
        }
    }
}
