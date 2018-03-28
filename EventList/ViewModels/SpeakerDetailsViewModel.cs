using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using MvvmHelpers;
using Xamarin.Forms;
using System.Linq;
using FormsToolkit;

namespace EventList.ViewModels
{
	public class SpeakerDetailsViewModel : ViewModelBase
	{
		public Speaker Speaker { get; set; }
	    public ObservableRangeCollection<MenuItem> FollowItems { get; } = new ObservableRangeCollection<MenuItem>();

		public SpeakerDetailsViewModel(Speaker speaker) : base()
		{
			Speaker = speaker;
			if (!string.IsNullOrWhiteSpace(speaker.CompanyWebsiteUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = "Web",
					Subtitle = speaker.CompanyWebsiteUrl,
					Parameter = speaker.CompanyWebsiteUrl
				});
			}

			if (!string.IsNullOrWhiteSpace(speaker.BlogUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = "Blog",
					Subtitle = speaker.BlogUrl,
					Parameter = speaker.BlogUrl
				});
			}

			if (!string.IsNullOrWhiteSpace(speaker.TwitterUrl))
			{
                FollowItems.Add(new MenuItem
				{
					Name = Device.OS == TargetPlatform.iOS ? "Twitter" : speaker.TwitterUrl,
					Subtitle = $"@{speaker.TwitterUrl}",
					Parameter = "http://twitter.com/" + speaker.TwitterUrl
				});
			}

			if (!string.IsNullOrWhiteSpace(speaker.LinkedInUrl))
			{
				FollowItems.Add(new MenuItem
				{
					Name = "LinkedIn",
					Subtitle = speaker.LinkedInUrl,
					Parameter = "http://linkedin.com/in/" + speaker.LinkedInUrl
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

		ICommand favoriteCommand;
		public ICommand FavoriteCommand =>
		favoriteCommand ?? (favoriteCommand = new Command<Session>((s) => ExecuteFavoriteCommand(s)));

		void ExecuteFavoriteCommand(Session session)
		{
			MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
			{
				Negative = "Cancel",
				Positive = "Unfavorite",
				Question = "Are you sure you want to remove this session from your favorites?",
				Title = "Unfavorite Session",
				//OnCompleted = (async (result) =>
					//{
					//	if (!result)
					//		return;

					//	var toggled = await FavoriteService.ToggleFavorite(session);
					//	if (toggled)
					//		await ExecuteLoadSessionsCommandAsync();
					//})
			});

		}

	}
}

