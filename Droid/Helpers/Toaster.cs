using System;
using Android.Widget;
using EventList.Droid.Helpers;
using EventList.Interfaces;
using Plugin.CurrentActivity;
using Xamarin.Forms;

[assembly: Dependency(typeof(Toaster))]
namespace EventList.Droid.Helpers
{
	public class Toaster : IToast
	{
		public void SendToast(string message)
		{
			var context = CrossCurrentActivity.Current.Activity ?? Android.App.Application.Context;
			Device.BeginInvokeOnMainThread(() =>
				{
					Toast.MakeText(context, message, ToastLength.Long).Show();
				});

		}
	}
}

