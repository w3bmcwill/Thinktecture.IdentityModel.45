/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Clients.AccessControlService
{
    public class IdentityProviderDiscoveryClient
    {
        public string AcsNamespace { get; set; }
        public string Realm { get; set; }
        public string Context { get; set; }

        public IdentityProviderDiscoveryClient(string acsNamespace, string realm)
        {
            AcsNamespace = acsNamespace;
            Realm = realm;
            Context = "";
        }

        public async Task<List<IdentityProviderInformation>> GetAsync(string protocol)
        {
            var url = string.Format(
                "https://{0}.{1}/v2/metadata/IdentityProviders.js?protocol={2}&realm={3}&context={4}&version=1.0",
                AcsNamespace,
                "accesscontrol.windows.net",
                protocol,
                Realm,
                Context);

            var jsonFormatter = new JsonMediaTypeFormatter();
            jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));

            var formatters = new List<MediaTypeFormatter>()
            {
                jsonFormatter
            };

            var client = new HttpClient();
            var response = await client.GetAsync(new Uri(url));

            return await response.Content.ReadAsAsync<List<IdentityProviderInformation>>(formatters);
        }

       
    }

    public class Protocols
    {
        public const string WSFederation = "wsfederation";
        public const string JavaScriptNotify = "javascriptnotify";
    }
}

