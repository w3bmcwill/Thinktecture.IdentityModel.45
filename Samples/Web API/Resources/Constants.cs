namespace Resources
{
    public static class Constants
    {
        public const string WebHost = "adfs.leastprivilege.vm";
        public const string SelfHost = "adfs.leastprivilege.vm:9000";
        public const string IdSrv = "identity.thinktecture.com";
        public const string ACS = "ttacssample.accesscontrol.windows.net";
        public const string ADFS = "adfs.leastprivilege.vm";

        public const string IdSrvSymmetricSigningKey = "Dc9Mpi3jbooUpBQpB/4R7XtUsa3D/ALSjTVvK8IUZbg=";
        public const string IdSrvSamlSigningKeyThumbprint = "A1EED7897E55388FCE60FEF1A1EED81FF1CBAEC6";
        public const string AcsSymmetricSigningKey = "yFvxu8Xkmo/xBSSPrzqZLSAiB4lgjR4PIi0Bn1RsUDI=";

        public const string IdSrvIssuerName = "http://identity.thinktecture.com/trust";

        public const string AdfsSamlSigningKeyThumbprint = "8EC7F962CC083FF7C5997D8A4D5ED64B12E4C174";

        public const string Realm = "https://samples.thinktecture.com/webapisecurity/";
        
        public const string WebHostBaseAddress = "https://" + WebHost + "/webapisecurity/api/";
        public const string SelfHostBaseAddress = "https://" + SelfHost + "/webapisecurity/api/";
    }
}
