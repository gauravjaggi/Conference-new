using EventList.Helpers;
using EventList.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace EventList.Pages.Chat
{
    public partial class GoogleLogin : ContentPage
    {
      //  private readonly GoogleViewModel _googleViewModel = new GoogleViewModel();

      //  public Button GoogleLoginButton { get; set; }

      //  public ChatRoomVieModel vm { get; set; }

        public GoogleLogin()
        {
            InitializeComponent();

            //BindingContext = vm = new ChatRoomVieModel(Navigation);

            //App.GoogleLoginPage = this;

            //if (Device.OS == TargetPlatform.Android)
            //{
                //Image logImage = new Image()
                //{
                //    Source = "Facebook.png",
                //    Aspect = Aspect.AspectFit,
                //    VerticalOptions = LayoutOptions.FillAndExpand,
                //    HorizontalOptions = LayoutOptions.Start
                //};

                //GoogleLoginButton = new Button()
                //{
                //    Text = "Login with facebook",
                //    HorizontalOptions = LayoutOptions.StartAndExpand,
                //    VerticalOptions = LayoutOptions.CenterAndExpand,
                //    TextColor = Color.White,
                //    BackgroundColor = Color.FromHex("#428edf")
                //    //BorderColor = Color.FromHex("#d3d3d3"),
                //    //BackgroundColor = Color.White,
                //    //WidthRequest = 180,
                //    //BorderWidth = 0.5,
                //    //BorderRadius = 2
                //};

                //GoogleLoginButton.Clicked += async delegate
                //{
                //    await Navigation.PushAsync(new GoogleLogin());
                //    //DependencyService.Get<IAndroidAuth>().Login();
                //};

                //Content = new StackLayout()
                //{
                //    HorizontalOptions = LayoutOptions.CenterAndExpand,
                //    VerticalOptions = LayoutOptions.CenterAndExpand,
                //    Orientation = StackOrientation.Horizontal,
                //    Spacing = 10,
                //    HeightRequest = 40,
                //    Children = { logImage, GoogleLoginButton }
                //};
           // }
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
