using System;
using System.Collections.Generic;
using EventList.Models;
using Xamarin.Forms;

namespace EventList.Pages.Info
{
    public partial class WifiInformationPage : ContentPage
    {
        ConferenceInfoViewModel vm;
        public WifiInformationPage(Wifi wifi)
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

            BindingContext = vm = new ConferenceInfoViewModel();
            vm.Settings.WiFiSSID = wifi.SSID;
            vm.Settings.WiFiPass = wifi.Password;
        }
		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await vm.UpdateConfigs();
		}
    }
}
