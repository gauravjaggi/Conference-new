using System;
using System.Collections.Generic;
using EventList.Helpers;
using EventList.Interfaces;
using EventList.Models;
using EventList.Pages.Chat;
using EventList.Pages.Favorites;
using EventList.Pages.Sessions;
using EventList.ViewModels;
using FormsToolkit;
using Xamarin.Forms;

namespace EventList.Pages.Home
{
    public partial class FeedsPage : ContentPage
    {

		FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);
		FeedViewModel vm;
		DateTime favoritesTime;
		string loggedIn;
        public FeedsPage()
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

              BindingContext =vm= new FeedViewModel(this.Navigation);
            vm.SelectedTab = CommonEnums.CommonEnums.Tab.Home;
			
			//ViewModel.Tweets.CollectionChanged += (sender, e) =>
			//{
			//	var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Tweets.Count + 2;
			//	ListViewSocial.HeightRequest = (ViewModel.Tweets.Count * ListViewSocial.RowHeight) - adjust;
			//};

			ViewModel.Sessions.CollectionChanged += (sender, e) =>
			{
				var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.Sessions.Count + 1;
				//ListViewSessions.HeightRequest = (ViewModel.Sessions.Count * ListViewSessions.RowHeight) - adjust;
			};
            lvPoiCategories.ItemTapped += (sender, e) => lvPoiCategories.SelectedItem = null;
            lvPoiCategories.ItemSelected +=  (sender, e) =>
            {
                var category = lvPoiCategories.SelectedItem as POICategory;
                if (category == null)
                    return;


                vm.SelectedPOICategory = category;
                vm.LoadPOISubCategoryCommand.Execute(null);
               
                lvPoiCategories.SelectedItem = null;
            };
            var inputalertservice = DependencyService.Get<IInputAlert>();
            string realmsettingjson = inputalertservice.GetSavedRealmSetting();
            if(!string.IsNullOrEmpty(realmsettingjson))
            {
                RealmSetting setting = Newtonsoft.Json.JsonConvert.DeserializeObject<RealmSetting>(realmsettingjson);
                App.CurrentRealmSetting = setting;
                vm.RealmDbConnectionCommand.Execute(null);
            }
            else // otherwise, open popup, ask for code, get realm settings
            {               
                ShowAlert(vm);
            }
        }
        async static public void ShowAlert(FeedViewModel vm)
        {
            var inputalertservice = DependencyService.Get<IInputAlert>();
            await inputalertservice.Show(vm);
        }
        public async void OnSettingsClicked(object sender,EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new SettingsPage(){Title="Settings"});
        }
	

		bool firstLoad = true;
		private void UpdatePage()
		{
            bool forceRefresh = (DateTime.UtcNow > (ViewModel?.NextForceRefresh ?? DateTime.UtcNow)) ||
			loggedIn != Settings.Current.Email;

			loggedIn = Settings.Current.Email;
			if (forceRefresh)
			{
				ViewModel.RefreshCommand.Execute(null);
				favoritesTime = Settings.Current.LastFavoriteTime;
			}
			else
			{

				if (ViewModel.Tweets.Count == 0)
				{

					ViewModel.LoadSocialCommand.Execute(null);
				}

				if ((firstLoad && ViewModel.Sessions.Count == 0) || favoritesTime != Settings.Current.LastFavoriteTime)
				{
					if (firstLoad)
						Settings.Current.LastFavoriteTime = DateTime.UtcNow;

					firstLoad = false;
					favoritesTime = Settings.Current.LastFavoriteTime;
					ViewModel.LoadSessionsCommand.Execute(null);
				}

				if (ViewModel.Notification == null)
					ViewModel.LoadNotificationsCommand.Execute(null);
			}

		}
    }
}
