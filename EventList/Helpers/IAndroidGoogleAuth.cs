using System;
using Xamarin.Forms;

namespace EventList.Helpers
{
    public interface IAndroidGoogleAuth
    {
		ContentPage context { get; set; }
		void Login();
		void Logout();
    }
}
