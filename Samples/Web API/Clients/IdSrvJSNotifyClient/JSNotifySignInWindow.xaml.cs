using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Newtonsoft.Json.Linq;
using Thinktecture.IdentityModel.Clients;

namespace Thinktecture.IdentityModel.Http.Wpf
{
    public partial class JSNotifySignInWindow : Window
    {
        public event EventHandler<SignedInEventArgs> SignedIn;

        public Uri EndpointUrl { get; set; }
        private SynchronizationContext _syncContext;

        public JSNotifySignInWindow()
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;
            
            var interop = new JavaScriptNotifyInterop();
            interop.ScriptNotify += this.OnScriptNotify;
            this.webBrowser.ObjectForScripting = interop;
        }

        public void SignIn()
        {
            this.Show();
            webBrowser.Navigate(EndpointUrl);
        }

        private void OnScriptNotify(object sender, ScriptNotifyEventArgs e)
        {
            OnSignedIn(JObject.Parse(e.Data));
            this.Close();
        }

        private void OnSignedIn(JObject data)
        {
            if (SignedIn != null)
            {
                var response = new AccessTokenResponse
                {
                    AccessToken = data["access_token"].ToString(),
                    ExpiresIn = int.Parse(data["expires_in"].ToString()),
                    TokenType = data["token_type"].ToString()
                };

                SignedIn(this, new SignedInEventArgs { Response = response });
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        [ComVisible(true)]
        public class JavaScriptNotifyInterop
        {
            public event EventHandler<ScriptNotifyEventArgs> ScriptNotify;

            public void Notify(string data)
            {
                OnScriptNotify(data);
            }

            protected virtual void OnScriptNotify(string data)
            {
                if (ScriptNotify != null)
                {
                    ScriptNotify.Invoke(this, new ScriptNotifyEventArgs { Data = data });
                }
            }
        }

        public class ScriptNotifyEventArgs : EventArgs
        {
            public string Data { get; set; }
        }

        public class SignedInEventArgs : EventArgs
        {
            public AccessTokenResponse Response { get; set; }
        }
    }
}
