//using System;
//using System.Linq;
//using CoreGraphics;
//using EventList.iOS.Renderers;
//using EventList.Models;
//using EventList.Pages.Chat;
//using Foundation;
//using Google.SignIn;
//using Realms.Sync;
//using UIKit;
//using Xamarin.Auth;
//using Xamarin.Forms;
//using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(GoogleLogin), typeof(SigninPageRenderer))]
//namespace EventList.iOS.Renderers
//{
//    public class SigninPageRenderer: Xamarin.Forms.Platform.iOS.PageRenderer, ISignInUIDelegate, ISignInDelegate
//    {
//        public GoogleLogin contentpage { get; set; }

//        public SigninPageRenderer()
//        {
//        }
//        protected override void OnElementChanged(Xamarin.Forms.Platform.iOS.VisualElementChangedEventArgs e)
//        {
//            base.OnElementChanged(e);
//            if (e.NewElement != null)
//            {
//                contentpage = e.NewElement as GoogleLogin;
//            }
//        }
//        static Random rand = new Random();
//        public async void DidSignIn(SignIn signIn, Google.SignIn.GoogleUser user, NSError error)
//        {
//            try
//            {
//                if (user != null)
//                {
//                    Color color = Color.FromRgb(rand.Next(255), rand.Next(255), rand.Next(255));
//                    var googleuser = new Models.GoogleUser()
//                    {
//                        Email = user.Profile.Email,
//                        FamilyName = user.Profile.FamilyName,
//                        GivenName = user.Profile.GivenName,
//                        UserId = user.UserID,
//                        BackgroundColor = new BackgroundColor()
//                        {
//                            R = color.R,
//                            G = color.G,
//                            B = color.B
//                        }
//                    };
//                    if (!App._Realm.All<Models.GoogleUser>().ToList().Any(x => x.UserId.Equals(user.UserID)))
//                    {
//                        App._Realm.Write(() =>
//                        {
//                            App._Realm.Add(googleuser);
//                        });
//                    }
//                    await contentpage.Navigation.PushAsync(new ChatRoomPage(googleuser) { Title = "Chat Window" });

//                }
//            }
//            catch(Exception ex)
//            {
                
//            }

//        }

//        public override void ViewDidLoad()
//        {
//            base.ViewDidLoad();

//            SignInButton btnSignIn = new SignInButton();

//            btnSignIn.Frame = new CGRect(20, View.Bounds.Height / 2 - 22, View.Bounds.Width - 40, 44);
//            View.AddSubview(btnSignIn);
//            SignIn.SharedInstance.UIDelegate = this;
//            SignIn.SharedInstance.Delegate = this;
//        }

//    }
//}
