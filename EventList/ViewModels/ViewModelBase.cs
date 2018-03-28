using System;
using MvvmHelpers;
using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using EventList.Interfaces;
using Plugin.Share;
using Plugin.Share.Abstractions;
using EventList.Services;
using EventList.Store;
using EventList.Models;

namespace EventList
{
	public class ViewModelBase : BaseViewModel
	{
		protected INavigation Navigation { get; }

        public CommonEnums.CommonEnums.Tab SelectedTab { get; set; }

		public ViewModelBase(INavigation navigation = null)
		{
			Navigation = navigation;
			//DependencyService.Register<IEventStore, EventStore>();
		}

        public void RaisePropertyChanged(string propertyName) {
            if (!string.IsNullOrEmpty(propertyName))
            {
                this.OnPropertyChanged(propertyName);
            }
        }


		public static void Init(bool mock = true)
		{
			
			DependencyService.Register<ISessionStore,SessionStore>();
			DependencyService.Register<IFavoriteStore,FavoriteStore>();
			DependencyService.Register<IFeedbackStore,FeedbackStore>();
			DependencyService.Register<ISpeakerStore, SpeakerStore>();
			DependencyService.Register<ISponsorStore, SponsorStore>();
			DependencyService.Register<ICategoryStore,CategoryStore>();
			DependencyService.Register<IEventStore, EventStore>();
			DependencyService.Register<INotificationStore, NotificationStore>();
			DependencyService.Register<IMiniHacksStore, MiniHacksStore>();			
			DependencyService.Register<IStoreManager, StoreManager>();

            DependencyService.Register<FavoriteService>();
		}

		protected IStoreManager StoreManager { get; } = DependencyService.Get<IStoreManager>();
		protected IToast Toast { get; } = DependencyService.Get<IToast>();
		protected FavoriteService FavoriteService { get; } = DependencyService.Get<FavoriteService>();


		public Settings Settings
		{
			get { return Settings.Current; }
		}

		ICommand launchBrowserCommand;
		public ICommand LaunchBrowserCommand =>
		launchBrowserCommand ?? (launchBrowserCommand = new Command<string>(async (t) => await ExecuteLaunchBrowserAsync(t)));

		async Task ExecuteLaunchBrowserAsync(string arg)
		{
			if (IsBusy)
				return;

			if (!arg.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !arg.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
				arg = "http://" + arg;			

			var lower = arg.ToLowerInvariant();

			if (Device.OS == TargetPlatform.iOS && lower.Contains("twitter.com"))
			{
				try
				{
					var id = arg.Substring(lower.LastIndexOf("/", StringComparison.Ordinal) + 1);
					var launchTwitter = DependencyService.Get<ILaunchTwitter>();
					if (lower.Contains("/status/"))
					{
						//status
						if (launchTwitter.OpenStatus(id))
							return;
					}
			  		else
					{
						//user
						if (launchTwitter.OpenUserName(id))
							return;
					}
				}
				catch
				{
				}
			}

			try
			{
				await CrossShare.Current.OpenBrowser(arg, new BrowserOptions
				{
					ChromeShowTitle = true,
					ChromeToolbarColor = new ShareColor
					{
						A = 255,
						R = 118,
						G = 53,
						B = 235
					},
                    UseSafariReaderMode = true,
					UseSafariWebViewController = true
				});
			}
            catch(Exception ex)
			{
			}
		}
		
	}
}