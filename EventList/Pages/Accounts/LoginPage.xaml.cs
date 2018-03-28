using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EventList.Pages.Accounts
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private async void LoginWithRealmBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Pages.Chat.LoginWithRealmPage());
        }

        private async void LoginWithGoogleBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.GoogleProfileCsPage());
        }

        private async void LoginWithFacebookBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.FacebookProfileCsPage());
        }
    }
}