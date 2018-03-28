using System;
using Android.App;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.OS;
using EventList.Droid.Services;
using EventList.Pages.Chat;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidGoogleAuth))]
namespace EventList.Droid.Services
{
    public class AndroidGoogleAuth:Java.Lang.Object,IAndroidAuth,GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        GoogleApiClient mGoogleApiClient;
       // GoogleLogin contentpage;
        public AndroidGoogleAuth()
        {
        }

        public ContentPage context { get; set ; }

        public void Login()
        {
            GoogleSignInOptions gso = new GoogleSignInOptions
                .Builder(GoogleSignInOptions.DefaultSignIn)
                .RequestIdToken("962249858669-815obt91d9su9bphcq47ljul4d6njk18.apps.googleusercontent.com").RequestProfile().RequestEmail().Build();

            //GoogleSignInOptions gso = new GoogleSignInOptions
            //    .Builder(GoogleSignInOptions.DefaultSignIn)
            //    .RequestIdToken("1070296876177-k70d2qa4ippbh70co70bbkgecf67hbj8.apps.googleusercontent.com").RequestProfile().RequestEmail().Build();

            mGoogleApiClient = new GoogleApiClient.Builder(((Activity)Forms.Context).ApplicationContext)
                                  .AddConnectionCallbacks(this)
                                  .AddOnConnectionFailedListener(this)
                                  .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                  .Build();

            mGoogleApiClient.Connect();

            var signInIntent =  Android.Gms.Auth.Api.Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);

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
           // throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            //throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            //throw new NotImplementedException();
        }
    }
}
