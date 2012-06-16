/*
 * Copyright (c) Dominick Baier.  All rights reserved.
 * see license.txt
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Thinktecture.IdentityModel.Clients.AccessControlService
{
    public class IdentityProviderDiscoveryClient
    {
        public string AcsNamespace { get; set; }
        public string Realm { get; set; }

        private WebClient _client;

        public event EventHandler<GetCompletedEventArgs> GetCompleted;

        public IdentityProviderDiscoveryClient(string acsNamespace, string realm)
        {
            AcsNamespace = acsNamespace;
            Realm = realm;

            _client = new WebClient();
            _client.DownloadStringCompleted += DownloadStringCompleted;
        }

        public void GetAsync(string protocol)
        {
            var url = string.Format("https://{0}.{1}/v2/metadata/IdentityProviders.js?protocol={2}&realm={3}&version=1.0",
                AcsNamespace,
                "accesscontrol.windows.net",
                protocol,
                Realm);

            _client.DownloadStringAsync(new Uri(url));
        }
        
        void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (GetCompleted != null)
            {
                IEnumerable<IdentityProviderInformation> idps = null;

                using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(e.Result)))
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IdentityProviderInformation[]));
                    idps = serializer.ReadObject(ms) as IEnumerable<IdentityProviderInformation>;

                    GetCompleted(this, new GetCompletedEventArgs(idps.ToList()));
                }
            }
        }

        public class GetCompletedEventArgs : EventArgs
        {
            List<IdentityProviderInformation> _idps;

            public List<IdentityProviderInformation> IdentityProvider
            {
                get { return _idps; }
            }

            public GetCompletedEventArgs(List<IdentityProviderInformation> idps)
            {
                _idps = idps;
            }
        }

        public class Protocols
        {
            public const string WSFederation = "wsfederation";
            public const string JavaScriptNotify = "javascriptnotify";
        }
    }
}
