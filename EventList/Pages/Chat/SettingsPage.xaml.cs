using System;
using System.Collections.Generic;
using EventList.Helpers;
using Xamarin.Forms;

namespace EventList.Pages.Chat
{
    public partial class SettingsPage : ContentPage
    {
        public Button GoogleLogin { get; set; }
        public SettingsPage()
        {
            InitializeComponent();
			if (Device.OS == TargetPlatform.Android)
			{
				GoogleLogin = new Button()
				{
					Image = "googleicon.png",
					Text = "Login with Google",
					HorizontalOptions = LayoutOptions.StartAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					BorderColor = Color.FromHex("#d3d3d3"),
					BackgroundColor = Color.White,
					WidthRequest = 200,
					BorderWidth = 0.5,
					BorderRadius = 2
				};
				GoogleLogin.Clicked += delegate
				{
					DependencyService.Get<IAndroidGoogleAuth>().Login();
				};
			
				Content = new StackLayout()
				{
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.CenterAndExpand,
					Orientation = StackOrientation.Vertical,
					Spacing = 10,
					Children = { GoogleLogin }
				};
			}
        }
    }
}
