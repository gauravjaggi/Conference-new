using Xamarin.Forms;
using EventList.Controls;
using EventList.Pages.iOS;
using EventList.Pages.Android;
using EventList.Pages.Windows;
using MvvmHelpers;
using EventList.Pages.Chat;
using EventList.Models;
using EventList.Interfaces;

namespace EventList
{
	public partial class App : Application
	{
        public static bool IsLocalNotifAllowed { get; set; }
       // public static GoogleLogin GoogleLoginPage { get; set; }
		public static Realms.Realm _Realm { get; set; }
        public static Realms.Realm _ChatRealm { get; set; }

        public static RealmSetting CurrentRealmSetting { get; set; }

		public App()
		{
			ViewModelBase.Init();
			InitializeComponent();

            MainPage = new AppNavigationPage(new RootPageiOS());	
        }     

	}
}
