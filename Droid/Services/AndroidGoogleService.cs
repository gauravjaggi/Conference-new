using System;
using Android.App;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using EventList.Helpers;
using EventList.Pages.Chat;
using Xamarin.Forms;

namespace EventList.Droid.Services
{
    public class AndroidGoogleService:Java.Lang.Object,IAndroidGoogleAuth, GoogleApiClient.IConnectionCallbacks// GoogleApiClient.IOnConnectionFailedListener
    {
		GoogleApiClient mGoogleApiClient;
		SettingsPage contentpage;
        public AndroidGoogleService()
        {
        }

        public ContentPage context { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Login()
        {
			GoogleSignInOptions gso = new GoogleSignInOptions
				.Builder(GoogleSignInOptions.DefaultSignIn)
				.RequestIdToken("497642192989-7j8dpedu6l57kh8anbg248pbo54hl7a2.apps.googleusercontent.com").RequestProfile().RequestEmail().Build();

			mGoogleApiClient = new GoogleApiClient.Builder(((Activity)Forms.Context).ApplicationContext)
								  .AddConnectionCallbacks(this)
								  .AddOnConnectionFailedListener(this)
								  .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
								  .Build();

			

			mGoogleApiClient.Connect();
			var signInIntent = Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);

			((Activity)Forms.Context).StartActivityForResult(signInIntent, 9001);
        }

        public async void Logout()
        {
			await Auth.GoogleSignInApi.SignOut(mGoogleApiClient);
			NavigateBack();
		}
		public async void NavigateBack()
		{
			await context.Navigation.PopAsync(true);
		}

        public void OnConnected(Bundle connectionHint)
        {
            //throw new NotImplementedException();
        }

        //public void OnConnectionFailed(GoConnectionResult result)
        //{
        //    //throw new NotImplementedException();
        //}

        //public void OnConnectionSuspended(int cause)
        //{
        //    //throw new NotImplementedException();
        //}
    }
}
