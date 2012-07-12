using System;
using System.Windows;
using Thinktecture.IdentityModel.Http.Wpf;
using System.Text;
using Thinktecture.IdentityModel.Clients;
using Thinktecture.Samples;
using Resources;
using System.Net.Http;
using System.Net.Http.Headers;

namespace IdSrvJSNotifyClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JSNotifySignInWindow _signin = new JSNotifySignInWindow
        {
            EndpointUrl = new Uri(string.Format("https://{0}/issue/jsnotify?realm={1}&tokenType={2}",
                                        Constants.IdSrv,
                                        Constants.Realm,
                                        "http://schemas.xmlsoap.org/ws/2009/11/swt-token-profile-1.0"))
        };

        AccessTokenResponse _response;

        public MainWindow()
        {
            InitializeComponent();
            _signin.SignedIn += new EventHandler<JSNotifySignInWindow.SignedInEventArgs>(_signin_SignedIn);
        }

        void _signin_SignedIn(object sender, JSNotifySignInWindow.SignedInEventArgs e)
        {
            _response = e.Response;
            _txtDebug.Text = e.Response.AccessToken;
        }

        private void _btnSignin_Click(object sender, RoutedEventArgs e)
        {
            _signin.SignIn();
        }

        private void _btnCallService_Click(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient { BaseAddress = new Uri(Constants.WebHostBaseAddress) };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("IdSrv", _response.AccessToken);

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var id = response.Content.ReadAsAsync<Identity>().Result;

            var sb = new StringBuilder(128);
            id.Claims.ForEach(c => sb.AppendFormat("{0}\n {1}\n\n", c.ClaimType, c.Value));
            _txtDebug.Text = sb.ToString();
        }
    }
}
