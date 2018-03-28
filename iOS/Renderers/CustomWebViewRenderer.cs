using System;
using EventList;
using EventList.iOS.Renderers;
using FormsToolkit.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


[assembly: Xamarin.Forms.ExportRenderer(typeof(EventList.Helpers.CustWebViewRenderer), typeof(CustomWebViewRenderer))]
namespace EventList.iOS.Renderers
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(){}

        //protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        //{

        //    base.OnElementChanged(e);
        //    if (e.OldElement != null || Element == null)
        //        return;

        //    Control.Settings.UserAgentString = "Chrome";
        //}
    }
}
