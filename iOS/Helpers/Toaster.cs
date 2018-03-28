using System;
using EventList.Interfaces;
using EventList.iOS.Helpers;
using ToastIOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]
namespace EventList.iOS.Helpers
{
	public class Toaster : IToast
	{
		public void SendToast(string message)
		{
			Device.BeginInvokeOnMainThread(() =>
				{
					Toast.MakeText(message, Toast.LENGTH_LONG).SetCornerRadius(0).Show();
				});
		}
	}
}
