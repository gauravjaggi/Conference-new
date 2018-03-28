using System;
using System.Linq;
using CoreGraphics;
using EventList.iOS.Renderers;
using EventList.Models;
using EventList.Pages.Chat;
using Foundation;
using Google.SignIn;
using Realms.Sync;
using UIKit;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ChatRoomPage), typeof(ChatRoomPageRenderer))]
namespace EventList.iOS.Renderers
{
    public class ChatRoomPageRenderer:Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public ChatRoomPageRenderer()
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.GestureRecognizers = null;
        }
    }
}
