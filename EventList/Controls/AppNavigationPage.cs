using System;

using Xamarin.Forms;

namespace EventList.Controls
{
	public class AppNavigationPage : NavigationPage
	{
		public AppNavigationPage(Page root) : base(root)
        {
			Init();
			Title = root.Title;
			Icon = root.Icon;
		}

		public AppNavigationPage()
		{
			Init();
		}

		void Init()
		{
            if (Device.OS == TargetPlatform.iOS || Device.OS== TargetPlatform.Android)
			{
                BarBackgroundColor = Color.FromHex("#428edf");//Color.FromHex("FAFAFA"); design change
                BarTextColor = Color.White;//design change
			}
			else
			{
				BarBackgroundColor = (Color)Application.Current.Resources["Primary"];
				BarTextColor = (Color)Application.Current.Resources["NavigationText"];
			}
		}
	}
}

