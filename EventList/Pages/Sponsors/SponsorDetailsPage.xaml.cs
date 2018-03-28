using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EventList.Pages.Sponsors
{
    public partial class SponsorDetailsPage : ContentPage
    {

        SponsorDetailsViewModel ViewModel => vm ?? (vm = BindingContext as SponsorDetailsViewModel);
        SponsorDetailsViewModel vm;
        public SponsorDetailsPage()
        {
            InitializeComponent();
        }
		public Sponsor Sponsor
		{
			get { return ViewModel.Sponsor; }
			set { BindingContext = new SponsorDetailsViewModel(Navigation, value); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			vm = null;
			var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.FollowItems.Count + 1;
			ListViewFollow.HeightRequest = (ViewModel.FollowItems.Count * ListViewFollow.RowHeight) - adjust;
		}
    }
}
