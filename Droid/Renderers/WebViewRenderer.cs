using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using EventList.Droid.Renderers;

[assembly: ExportRenderer(typeof(EventList.Helpers.CustWebViewRenderer), typeof(CustomWebViewRenderer))]
namespace EventList.Droid.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {

            base.OnElementChanged(e);
            if (e.OldElement != null || Element == null)
                return;

            Control.Settings.UserAgentString = "Chrome";
        }
    }
}