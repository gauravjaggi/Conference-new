using EventList.Models;
using Realms.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EventList.Pages.Chat
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginWithRealmPage : ContentPage
    {
        public LoginWithRealmPage()
        {
            InitializeComponent();
        }

        private async void LoginWithRealm_Clicked(object sender, EventArgs e)
        {
            string userName = txt_Email.Text;

            string password = txt_Password.Text;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)
                && (App.CurrentRealmSetting.UserName == userName && App.CurrentRealmSetting.Password == password))
            {
                var credentials = Credentials.UsernamePassword(userName, password, false);

                User userData = await User.LoginAsync(credentials, new Uri($"http://{App.CurrentRealmSetting.SyncHost}"));

                if (userData != null && !string.IsNullOrEmpty(userData.Identity) && userData.IsAdmin)
                {
                    //GoogleUser googleUser = App._ChatRealm.Find<GoogleUser>(userData.Identity);
                    GoogleUser googleUser = new GoogleUser()
                    {
                        UserId = userData.Identity,
                        Email = userName,
                        GivenName = "Admin",
                        FamilyName = ""
                    };

                    if (googleUser != null)
                    {
                        await Navigation.PushAsync(new ChatRoomPage(googleUser) { Title = "Chat Window" });
                    }
                    else
                    {
                        await DisplayAlert("Message", "Please fill correct email and password.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Message", "Please fill correct email and password.", "Ok");
                }
            }
            else
            {
                await DisplayAlert("Message", "Please fill correct email and password.", "Ok");
            }
        }
    }
}