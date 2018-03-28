using System;
using EventList;
using EventList.iOS.Renderers;
using FormsToolkit.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(RoundedStack), typeof(RoundStackRenderer))]
namespace EventList.iOS.Renderers
{
    public class RoundStackRenderer:ViewRenderer{
        public RoundStackRenderer()
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            this.BackgroundColor = UIColor.FromRGB(40,121,255);
            this.Layer.CornerRadius = 20f;
            this.Layer.BorderColor = Color.FromHex("#2879ff").ToCGColor();
            this.Layer.BorderWidth = 1;
        }
    }
}
