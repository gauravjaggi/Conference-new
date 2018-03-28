using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.ViewModels;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace EventList.Pages.CommonEvents
{
    public partial class MyNetworks : ContentPage
    {
        QRCodeViewModel vm;
        public MyNetworks()
        {
            InitializeComponent();
            BindingContext = vm = new QRCodeViewModel(Navigation);
            lvCodes.ItemTapped += (sender, e) => lvCodes.SelectedItem = null;
            lvCodes.ItemSelected += async (sender, e) =>
            {

                QRCode ev = lvCodes.SelectedItem as QRCode;

                if (ev == null)
                    return;
                await this.Navigation.PushAsync(new QRCodeDetailPage(ev.Code));

                lvCodes.SelectedItem = null;
            };
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadQRCodesCommand.Execute(null);
        }
        public async void OnAddClicked(object sender, EventArgs e)
        {
            ZXingScannerPage scanPage = new ZXingScannerPage();
            scanPage.HasTorch = true;
            scanPage.OnScanResult += (result) =>
            {
                scanPage.IsScanning = false;

                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    vm.AddQRCodesCommand.Execute(result.Text);
                });
            };
            await Navigation.PushAsync(scanPage);
        }
    }
}
