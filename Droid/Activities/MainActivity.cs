using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;
using EventList.Droid.Services;
using System.Collections.Generic;
using Refractored.XamForms.PullToRefresh;
using Android.Gms.Auth.Api.SignIn;
using EventList.Pages.Chat;
using System.Drawing;
using EventList.Models;
using System.Linq;
using Android.Gms.Auth.Api;

namespace EventList.Droid
{
	[Activity(Label = "EventList.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
        private Android.Webkit.WebView webView;
        
       // public static String USER_AGENT_FAKE = "Mozilla/5.0 (Linux; Android 4.1.1; Galaxy Nexus Build/JRO03C) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";
        public static List<Intent> EventAlarmIntents;

		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

            ZXing.Net.Mobile.Forms.Android.Platform.Init();

            global::Xamarin.Forms.Forms.Init                                                                                                                                                                                                                   (this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);

			//PullToRefreshLayoutRenderer.Init();
            ImageCircle.Forms.Plugin.Droid.ImageCircleRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);
          //  webView.Settings.UserAgentString = "Chrome";
            LoadApplication(new App());                         
		}


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
		{			
			PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}

        static Random rand = new Random();

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == 9001)
            {
                this.FinishActivity(requestCode);

                var color = GetRandomColor();

                GoogleSignInResult result = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInResultFromIntent(data);

                if (result != null && result.IsSuccess)
                {

                    var googleuser = new Models.GoogleUser()
                    {
                        Email = result.SignInAccount.Email,
                        FamilyName = result.SignInAccount.DisplayName,
                        GivenName = result.SignInAccount.DisplayName,
                        UserId = result.SignInAccount.Id,
                        BackgroundColor = new BackgroundColor()
                        {
                            R = color.R,
                            G = color.G,
                            B = color.B
                        }
                    };
                    if (!App._Realm.All<Models.GoogleUser>().ToList().Any(x => x.UserId.Equals(result.SignInAccount.Id)))
                    {
                        App._Realm.Write(() =>
                        {
                            App._Realm.Add(googleuser);
                        });
                    }
                 //   App.GoogleLoginPage.Navigation.PushAsync(new ChatRoomPage(googleuser) { Title = "Chat Window" });
                }
            }
        }

        public Android.Graphics.Color GetRandomColor()
        {
            int hue = rand.Next(255);
            Android.Graphics.Color color = Android.Graphics.Color.HSVToColor(
                new[] {
            hue,
            1.0f,
            1.0f,
                }
            );
            return color;
        }
    }
}
