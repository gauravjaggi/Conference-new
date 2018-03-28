using System;
using CoreGraphics;
using EventList.iOS.Renderers;
using Foundation;
using Google.SignIn;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(EventList.Pages.Chat.SettingsPage), typeof(SettingsPageRenderer))]
namespace EventList.iOS.Renderers
{
    public class SettingsPageRenderer : PageRenderer, ISignInUIDelegate, ISignInDelegate
    {
        public SettingsPageRenderer()
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			SignInButton btnSignIn = new SignInButton();

			btnSignIn.Frame = new CGRect(20, 20, View.Bounds.Width - 40, 44);
			View.AddSubview(btnSignIn);
			SignIn.SharedInstance.UIDelegate = this;
			SignIn.SharedInstance.Delegate = this;

        }
        public void DidSignIn(SignIn signIn, GoogleUser user, NSError error)
        {
            if (user != null)
            {


            }
        }
    }
}
