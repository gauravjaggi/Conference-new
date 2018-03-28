using System;
using Xamarin.Forms;

namespace EventList
{
	public class CardView : Frame
	{
        public CardView()
        {
            Padding = 0;
            //if (Device.OS == TargetPlatform.iOS)
            //{
            //HasShadow = false;
            OutlineColor = Color.Transparent;
            BackgroundColor = Color.FromHex("#428edf");
            CornerRadius = 8;
            //}
        }
	}
}

