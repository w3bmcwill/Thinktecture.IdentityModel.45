using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using AcsJsNotifyClient;
using Resources;
using Thinktecture.IdentityModel.Clients.AccessControlService;
using Thinktecture.Samples;


namespace AcsJSNotifyClient
{
    public partial class MainWindow : Window
    {
        public JSNotifyRequestSecurityTokenResponse RSTR { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void _btnSignin_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AcsSignInWindow()
            {
                AcsNamespace = "ttacssample",
                Realm = Constants.Realm,
                Owner = this
            };

            if (dialog.ShowDialog().Value)
            {
                RSTR = dialog.Response;
                _txtDebug.Text = RSTR.SecurityTokenString;
            }
        }

        private void _btnCallService_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient { BaseAddress = new Uri(Constants.WebHostBaseAddress) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("ACS", RSTR.SecurityTokenString);

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var id = response.Content.ReadAsAsync<Identity>().Result;

            var sb = new StringBuilder(128);
            id.Claims.ForEach(c => sb.AppendFormat("{0}\n {1}\n\n", c.ClaimType, c.Value));
            _txtDebug.Text = sb.ToString();
        }
    }
}