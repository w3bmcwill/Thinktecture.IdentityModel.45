/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System.Web.Http;

namespace Thinktecture.IdentityModel.WebApi
{
    public static class HttpConfigurationExtensions
    {
        public static void SetAuthorizationManager(this HttpConfiguration configuration, IAuthorizationManager manager)
        {
            configuration.Properties[ApiAuthorizeAttribute.PropertyName] = manager;
        }
    }
}
