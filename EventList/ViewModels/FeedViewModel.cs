using System;
using MvvmHelpers;
using EventList.Models;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using FormsToolkit;
using EventList.Services;
using Realms.Sync;
using System.Linq;
using EventList.Extensions;
using EventList.Pages.CommonEvents;
//using EventList.Extensions;

namespace EventList.ViewModels
{
	public class FeedViewModel : ViewModelBase
	{
		public Realms.Sync.User RealmAdmin { get; set; }
		public ObservableRangeCollection<Tweet> Tweets { get; } = new ObservableRangeCollection<Tweet>();

        public ObservableRangeCollection<EventCategory> EventCategories { get; set; }

        public ObservableRangeCollection<POICategory> POICategories { get; } = new ObservableRangeCollection<POICategory>();
        public ObservableRangeCollection<Grouping<string, POICategory>> CategoriesGrouped { get; } = new ObservableRangeCollection<Grouping<string, POICategory>>();

        public ObservableRangeCollection<POISubCategory> POISubCategories { get; set; } = new ObservableRangeCollection<POISubCategory>();
        public ObservableRangeCollection<Grouping<string, POISubCategory>> POISubCategoriesGrouped { get; } = new ObservableRangeCollection<Grouping<string, POISubCategory>>();

        public ObservableRangeCollection<POIItem> POIItems { get; set; } = new ObservableRangeCollection<POIItem>();


		public ObservableRangeCollection<Models.Session> Sessions { get; } = new ObservableRangeCollection<Models.Session>();
		public DateTime NextForceRefresh { get; set; }

        public INavigation _navigation { get; set; }
        public FeedViewModel(INavigation navigation) : base(navigation)
		{
            _navigation = navigation;
			NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
            EventCategories = new ObservableRangeCollection<EventCategory>();

   //         EventCategories.Add(new EventCategory() {
   //             Name = "Schedule",
   //             Image = "Schedule.png",
   //         });
			//EventCategories.Add(new EventCategory()
			//{
			//	Name = "Favorites",
   //              Image = "Favorites.png"
   //         });
			//EventCategories.Add(new EventCategory()
			//{
			//	Name = "Sessions",
   //             Image = "Sessions.png"
   //         });
			//EventCategories.Add(new EventCategory()
			//{
			//	Name = "Featured Events",
   //             Image = "FeaturedEvents.png"
   //         });
			//EventCategories.Add(new EventCategory()
			//{
			//	Name = "Mini-Hacks",
   //             Image = "MiniHacks.png"
   //         });
			//EventCategories.Add(new EventCategory()
			//{
			//	Name = "Speakers",
   //             Image = "Speakers.png"
   //         });
			//EventCategories.Add(new EventCategory()
			//{
			//	Name = "Attendees",
   //             Image = "Attendees.png"
   //         });
   //         EventCategories.Add(new EventCategory()
   //         {
   //             Name = "My Network",
   //             Image = "MyNetwork.png"
   //         });
		}


		ICommand refreshCommand;
		public ICommand RefreshCommand =>
			refreshCommand ?? (refreshCommand = new Command(async () => await ExecuteRefreshCommandAsync()));

		async Task ExecuteRefreshCommandAsync()
		{
			try
			{
				NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
				IsBusy = true;
				var tasks = new Task[]
					{
						ExecuteLoadNotificationsCommandAsync(),
						ExecuteLoadSocialCommandAsync(),
						ExecuteLoadSessionsCommandAsync()
					};

				await Task.WhenAll(tasks);
			}
			catch (Exception ex)
			{
				ex.Data["method"] = "ExecuteRefreshCommandAsync";
			}
			finally
			{
				IsBusy = false;
			}
		}

		Notification notification;
		public Notification Notification
		{
			get { return notification; }
			set { SetProperty(ref notification, value); }
		}

		bool loadingNotifications;
		public bool LoadingNotifications
		{
			get { return loadingNotifications; }
			set { SetProperty(ref loadingNotifications, value); }
		}

		ICommand loadNotificationsCommand;
		public ICommand LoadNotificationsCommand =>
			loadNotificationsCommand ?? (loadNotificationsCommand = new Command(async () => await ExecuteLoadNotificationsCommandAsync()));

		async Task ExecuteLoadNotificationsCommandAsync()
		{
			if (LoadingNotifications)
				return;
			LoadingNotifications = true;
#if DEBUG
			await Task.Delay(1000);
#endif

			try
			{
				Notification = await StoreManager.NotificationStore.GetLatestNotification();
			}
			catch (Exception ex)
			{
				ex.Data["method"] = "ExecuteLoadNotificationsCommandAsync";
				Notification = new Notification
				{
					Date = DateTime.UtcNow,
					Text = "Welcome to Xamarin Evolve!"
				};
			}
			finally
			{
				LoadingNotifications = false;
			}
		}

		bool loadingSessions;
		public bool LoadingSessions
		{
			get { return loadingSessions; }
			set { SetProperty(ref loadingSessions, value); }
		}


		ICommand loadSessionsCommand;
		public ICommand LoadSessionsCommand =>
			loadSessionsCommand ?? (loadSessionsCommand = new Command(async () => await ExecuteLoadSessionsCommandAsync()));

		async Task ExecuteLoadSessionsCommandAsync()
		{
			if (LoadingSessions)
				return;

			LoadingSessions = true;

			try
			{
				NoSessions = false;
				Sessions.Clear();
				OnPropertyChanged("Sessions");
#if DEBUG
				await Task.Delay(1000);
#endif
				var sessions = await StoreManager.SessionStore.GetNextSessions();
				if (sessions != null)
					Sessions.AddRange(sessions);

				NoSessions = Sessions.Count == 0;
			}
			catch (Exception ex)
			{
				ex.Data["method"] = "ExecuteLoadSessionsCommandAsync";
				NoSessions = true;
			}
			finally
			{
				LoadingSessions = false;
			}

		}

		bool noSessions;
		public bool NoSessions
		{
			get { return noSessions; }
			set { SetProperty(ref noSessions, value); }
		}

		Models.Session selectedSession;
		public Models.Session SelectedSession
		{
			get { return selectedSession; }
			set
			{
				selectedSession = value;
				OnPropertyChanged();
				if (selectedSession == null)
					return;

				//MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, selectedSession);

				SelectedSession = null;
			}
		}

		bool loadingSocial;
		public bool LoadingSocial
		{
			get { return loadingSocial; }
			set { SetProperty(ref loadingSocial, value); }
		}


		ICommand loadSocialCommand;
		public ICommand LoadSocialCommand =>
			loadSocialCommand ?? (loadSocialCommand = new Command(async () => await ExecuteLoadSocialCommandAsync()));

		async Task ExecuteLoadSocialCommandAsync()
		{
			if (LoadingSocial)
				return;

			LoadingSocial = true;
			try
			{
				SocialError = false;
				Tweets.Clear();

				using (var client = new HttpClient())
				{
#if ENABLE_TEST_CLOUD
                                        var json = ResourceLoader.GetEmbeddedResourceString(Assembly.Load(new AssemblyName("XamarinEvolve.Clients.Portable")), "sampletweets.txt");
                                        Tweets.ReplaceRange(JsonConvert.DeserializeObject<List<Tweet>>(json));
#else


					var manager = DependencyService.Get<IStoreManager>() as StoreManager;// as XamarinEvolve.DataStore.Azure.StoreManager;
					if (manager == null)
						return;

					await manager.InitializeAsync();

					var mobileClient = EventList.StoreManager.MobileService;
					if (mobileClient == null)
						return;

					var json = await mobileClient.InvokeApiAsync<string>("Tweet", System.Net.Http.HttpMethod.Get, null);

					if (string.IsNullOrWhiteSpace(json))
					{
						SocialError = true;
						return;
					}


					Tweets.ReplaceRange(JsonConvert.DeserializeObject<List<Tweet>>(json));
#endif
				}

			}
			catch (Exception ex)
			{
				SocialError = true;
				ex.Data["method"] = "ExecuteLoadSocialCommandAsync";
			}
			finally
			{
				LoadingSocial = false;
			}

		}

		bool socialError;
		public bool SocialError
		{
			get { return socialError; }
			set { SetProperty(ref socialError, value); }
		}

		Tweet selectedTweet;
		public Tweet SelectedTweet
		{
			get { return selectedTweet; }
			set
			{
				selectedTweet = value;
				OnPropertyChanged();
				if (selectedTweet == null)
					return;

				Device.OpenUri(new Uri(selectedTweet.Url));
				//LaunchBrowserCommand.Execute(selectedTweet.Url);

				SelectedTweet = null;
			}
		}

		ICommand favoriteCommand;
		public ICommand FavoriteCommand =>
		favoriteCommand ?? (favoriteCommand = new Command<Models.Session>((s) => ExecuteFavoriteCommand(s)));

		void ExecuteFavoriteCommand(Models.Session session)
		{
			MessagingService.Current.SendMessage<MessagingServiceQuestion>(MessageKeys.Question, new MessagingServiceQuestion
			{
				Negative = "Cancel",
				Positive = "Unfavorite",
				Question = "Are you sure you want to remove this session from your favorites?",
				Title = "Unfavorite Session",
				OnCompleted = (async (result) =>
					{
						if (!result)
							return;

						var toggled = await FavoriteService.ToggleFavorite(session);
						if (toggled)
							await ExecuteLoadSessionsCommandAsync();
					})
			});

		}
        ICommand loadPOIDataCommand;
        public ICommand LoadPOIDataCommand =>
        loadPOIDataCommand ?? (loadPOIDataCommand = new Command(async () => await ExecuteLoadPOIDataCommandAsync()));

        async Task ExecuteLoadPOIDataCommandAsync()
        {
            IsBusy = true;
            try
            {
                List<POICategory> categories= new List<POICategory>();
                if (App._Realm != null)
                {
                    var poicategories = App._Realm.All<POICategory>();
                    if (poicategories.Count().Equals(0))
                    {
                        categories = await APIServices.GetPOICategories();

                        foreach (POICategory item in categories)
                        {
                            var cats = App._Realm.All<POICategory>();
                            if (!cats.Any(x => x.CategoryID == item.CategoryID))
                            {
                                App._Realm.Write(() =>
                                {
                                    App._Realm.Add(item);
                                });
                            }
                        }
                        foreach (var item in EventCategories)
                        {

                            POICategory oldItem = new POICategory()
                            {
                                CategoryID = 0,
                                Tab = "events",
                                TableSection = string.Empty,
                                Title = item.Name,
                                Image = item.Image,
                                SubTitle = String.Empty
                            };
                            categories.Add(oldItem);
                        }
                        switch (SelectedTab)
                        {
                            case CommonEnums.CommonEnums.Tab.Home:
                                POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("home")));
                                break;
                            case CommonEnums.CommonEnums.Tab.Events:
                              
                                POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("events")));
                                break;
                            case CommonEnums.CommonEnums.Tab.Info:
                                POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("info")));
                                break;
                        }

                    }
                    else
                    {
                        foreach (var item in poicategories)
                        {
                            if (!categories.Any(x => x.CategoryID == item.CategoryID))
                                categories.Add(item);
                        }

                        foreach (var eventcategory in EventCategories)
                        {
                            POICategory cat = new POICategory()
                            {
                                CategoryID = 0,
                                Tab = "events",
                                TableSection = string.Empty,
                                Title = eventcategory.Name,
                                Image=eventcategory.Image,
                                SubTitle = string.Empty
                            };
                            categories.Add(cat);
                        }

                        switch (SelectedTab)
                        {
                            case CommonEnums.CommonEnums.Tab.Home:
                                POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("home")));
                                break;
                            case CommonEnums.CommonEnums.Tab.Events:
                                POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("events")));
                                break;
                            case CommonEnums.CommonEnums.Tab.Info:
                                POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("info")));
                                break;
                        }
                    }
                    CategoriesGrouped.ReplaceRange(POICategories.GroupBySection());
                }
            }
            catch(Exception ex)
            {
                
            }
            finally
            {
                IsBusy = false;
            }
        }
        public POICategory SelectedPOICategory { get; set; }

        ICommand loadPOISubCategoryCommand;
        public ICommand LoadPOISubCategoryCommand =>
        loadPOISubCategoryCommand ?? (loadPOISubCategoryCommand = new Command(async () => await ExecuteLoadPOISubCategoryCommandAsync()));

        async Task ExecuteLoadPOISubCategoryCommandAsync()
        {
            try
            {
                var poisubcategories = App._Realm.All<POISubCategory>();
                if (poisubcategories.Count().Equals(0))
                {
                    var subCategories = await APIServices.GetPOISubCategories();
                    foreach (POISubCategory item in subCategories)
                    {
                        var subcats = App._Realm.All<POISubCategory>();
                        if (!subcats.Any(x => x.SubCategoryID==item.SubCategoryID))
                        {
                            App._Realm.Write(() =>
                            {
                                App._Realm.Add(item);
                            });
                        }
                    }
                    POISubCategories = new ObservableRangeCollection<POISubCategory>();
                    foreach (var item in subCategories)
                    {
                        if (item.CategoryId == SelectedPOICategory.CategoryID)
                        {
                            POISubCategories.Add((item));
                        }
                    }
                }
                else
                {
                    POISubCategories = new ObservableRangeCollection<POISubCategory>();
                    foreach (var item in poisubcategories)
                    {
                        if (item.CategoryId == SelectedPOICategory.CategoryID)
                        {
                            POISubCategories.Add((item));
                        }
                    }
                }
                POISubCategoriesGrouped.ReplaceRange(POISubCategories.GroupBySection());
                if (POISubCategoriesGrouped.Count > 0)
                {
                    await _navigation.PushAsync(new POISubCategoryPage(POISubCategoriesGrouped));
                }
            }
            catch (Exception ex)
            {

            }
        }
        public POISubCategory SelectedPOISubCategory { get; set; }

        ICommand loadPOIItemCommand;
        public ICommand LoadPOIItemCommand =>
        loadPOIItemCommand ?? (loadPOIItemCommand = new Command(async () => await ExecuteLoadPOIItemCommandAsync()));

        async Task ExecuteLoadPOIItemCommandAsync()
        {
            try
            {
                var poiitems = App._Realm.All<POIItem>();
                if (poiitems.Count().Equals(0))
                {
                    var items = await APIServices.GetPOIItems();
                    foreach (POIItem item in items)
                    {
                        App._Realm.Write(() =>
                        {
                            App._Realm.Add(item);
                        });
                    }
                    POIItems = new ObservableRangeCollection<POIItem>();
                    foreach (var item in items)
                    {
                        if (item.SubCategoryId == SelectedPOISubCategory.SubCategoryID)
                        {
                            POIItems.Add((item));
                        }
                    }
                }
                else
                {
                    POIItems = new ObservableRangeCollection<POIItem>();
                    foreach (var item in poiitems)
                    {
                        if (item.SubCategoryId == SelectedPOISubCategory.SubCategoryID)
                        {
                            POIItems.Add((item));
                        }
                    }
                }
                if (POIItems!=null && POIItems.Count > 0)
                {
                    await _navigation.PushAsync(new POIItemPage(POIItems, SelectedPOISubCategory.Title));
                }
            }
            catch (Exception ex)
            {

            }
        }
		public Realms.Sync.User ChatRealmAdmin { get; set; }

		ICommand realmDbConnectionCommand;
		public ICommand RealmDbConnectionCommand =>
			realmDbConnectionCommand ?? (realmDbConnectionCommand = new Command(async () => await ExecuteRealmDbConnectionCommand()));

        async Task ExecuteRealmDbConnectionCommand()
		{
			try
			{
				IsBusy = true;
                RealmAdmin = User.Current;
                ChatRealmAdmin = User.Current;
                var syncuri = new Uri($"http://{App.CurrentRealmSetting.SyncHost}");
                if(RealmAdmin ==null)
                {
                    var credentials = Credentials.UsernamePassword(Constants.Username, Constants.Password, false);
                    RealmAdmin = await User.LoginAsync(credentials, syncuri);
                }
                var config = new SyncConfiguration(RealmAdmin,new Uri($"realm://{App.CurrentRealmSetting.SyncHost}/~/{App.CurrentRealmSetting.SyncServerUri}"))
				{
					ObjectClasses = new[] {
                        typeof(Models.Session),
                        typeof(Models.Speaker),
                        typeof(Models.Room),
                        typeof(Models.Category),
                        typeof(FeaturedEvent),
                        typeof(Sponsor),
                        typeof(SponsorLevel),
                        typeof(MiniHack),
                        typeof(Favorite),
                        typeof(Feedback),
                        typeof(Attendee),
                        typeof(QRCode),
                        typeof(Models.GoogleUser),
                        typeof(BackgroundColor),
                        typeof(POICategory),
                        typeof(POISubCategory),
                        typeof(POIItem)}
				};
				App._Realm = Realms.Realm.GetInstance(config);
				if (ChatRealmAdmin == null)
				{
                    var ccredentials = Credentials.UsernamePassword(App.CurrentRealmSetting.UserName, App.CurrentRealmSetting.Password, false);
                    ChatRealmAdmin = await User.LoginAsync(ccredentials, syncuri);

				}
                var chatconfig = new SyncConfiguration(ChatRealmAdmin,new Uri($"realm://{App.CurrentRealmSetting.SyncHost}/~/{App.CurrentRealmSetting.ChatSyncServerUri}"))
				{
                    ObjectClasses = new[] { typeof(Message) }
				};
				App._ChatRealm = Realms.Realm.GetInstance(chatconfig);
                Application.Current.Properties["realm"] = App.CurrentRealmSetting;
                LoadPOIDataCommand.Execute(null);
			}
			catch (Exception ex)
			{

			}
			finally
			{
				IsBusy = false;
			}
		}

        async public Task<List<RealmSetting>> GetRealmSettings(string text)
        {
            IsBusy = true;
            List<RealmSetting> result = null;
            try{
                 result = await APIServices.GetRealmSettings(text);
            }
            catch (Exception ex){
                
            }
            finally{
                IsBusy = false;
            } 
            return result;
        }
	}
}
