using System;
using System.Windows.Input;
using EventList.Models;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList
{
    public class AttendeesDetailViewModel:ViewModelBase
    {
		public Attendee Attendee { get; set; }
		public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

		public AttendeesDetailViewModel(Attendee attendee) : base()
        {
			Attendee = attendee;
			if (!string.IsNullOrWhiteSpace(attendee.CompanyWebsiteUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = "Web",
					Subtitle = attendee.CompanyWebsiteUrl,
					Parameter = attendee.CompanyWebsiteUrl
				});
			}

			if (!string.IsNullOrWhiteSpace(attendee.BlogUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = "Blog",
					Subtitle = attendee.BlogUrl,
					Parameter = attendee.BlogUrl
				});
			}

			if (!string.IsNullOrWhiteSpace(attendee.TwitterUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = Device.OS == TargetPlatform.iOS ? "Twitter" : attendee.TwitterUrl,
					Subtitle = $"@{attendee.TwitterUrl}",
					Parameter = "http://twitter.com/" + attendee.TwitterUrl
				});
			}

			if (!string.IsNullOrWhiteSpace(attendee.LinkedInUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = "LinkedIn",
					Subtitle = attendee.LinkedInUrl,
					Parameter = "http://linkedin.com/in/" + attendee.LinkedInUrl
				});
			}

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
