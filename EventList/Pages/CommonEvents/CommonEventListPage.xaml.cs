using System;
using System.Collections.Generic;
using EventList.Helpers;
using EventList.Models;
using EventList.Pages.Events;
using EventList.Pages.Favorites;
using EventList.Pages.MiniHacks;
using EventList.Pages.Schedule;
using EventList.Pages.Sessions;
using EventList.Pages.Speakers;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.CommonEvents
{
    public partial class CommonEventListPage : ContentPage
    {
		FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);
		FeedViewModel vm;
        public CommonEventListPage()
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

            BindingContext =vm= new FeedViewModel(this.Navigation);
            lvPoiCategories.ItemTapped += (sender, e) => lvPoiCategories.SelectedItem = null;
            lvPoiCategories.ItemSelected += async (sender, e) =>
			{
                var category = lvPoiCategories.SelectedItem as POICategory;
				if (category == null)
					return;
                
                if(category.CategoryID.Equals(0))
                {
                    if(category.Title.Equals("Schedule"))
                        await NavigationService.PushAsync(Navigation, new SchedulePage());
                    if (category.Title.Equals("Favorites"))
                        await NavigationService.PushAsync(Navigation, new FavoritesPage());
                    if (category.Title.Equals("Sessions"))
                        await NavigationService.PushAsync(Navigation, new SessionsPage());
                    if (category.Title.Equals("Featured Events"))
                        await NavigationService.PushAsync(Navigation, new EventListPage());
                    if (category.Title.Equals("Mini-Hacks"))
                        await NavigationService.PushAsync(Navigation, new MiniHacksPage());
                    if (category.Title.Equals("Speakers"))
                        await NavigationService.PushAsync(Navigation, new SpeakersPage());
                    if (category.Title.Equals("Attendees"))
                        await NavigationService.PushAsync(Navigation, new AttendeesPage());
                    if (category.Title.Equals("My Network"))
                        await NavigationService.PushAsync(Navigation, new MyNetworks());
                }
                else{
                    vm.SelectedPOICategory = category;
                    vm.LoadPOISubCategoryCommand.Execute(null);
                }
				
                lvPoiCategories.SelectedItem = null;
			};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.SelectedTab = CommonEnums.CommonEnums.Tab.Events;
            vm.LoadPOIDataCommand.Execute(null);
        }
    }
}
