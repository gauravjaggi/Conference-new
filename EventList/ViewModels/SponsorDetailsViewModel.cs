using System;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList
{
    public class SponsorDetailsViewModel:ViewModelBase
    {
		public Sponsor Sponsor { get; }
		public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

		public SponsorDetailsViewModel(INavigation navigation, Sponsor sponsor) : base(navigation)
		{
			Sponsor = sponsor;
			FollowItems.Add(new MenuItem
			{
				Name = "Web",
				Subtitle = sponsor.WebsiteUrl,
				Parameter = sponsor.WebsiteUrl,
			});
			FollowItems.Add(new MenuItem
			{
				Name = Device.OS == TargetPlatform.iOS ? "Twitter" : sponsor.TwitterUrl,
				Subtitle = $"@{sponsor.TwitterUrl}",
				Parameter = "http://twitter.com/" + sponsor.TwitterUrl,
			
			});
		}

		MenuItem selectedFollowItem;
		public MenuItem SelectedFollowItem
		{
			get { return selectedFollowItem; }
			set
			{
				selectedFollowItem = value;
				OnPropertyChanged();
				if (selectedFollowItem == null)
					return;

				LaunchBrowserCommand.Execute(selectedFollowItem.Parameter);

				SelectedFollowItem = null;
			}
		}
    }
}
