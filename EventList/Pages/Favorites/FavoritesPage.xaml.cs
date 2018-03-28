using System;
using System.Collections.Generic;
using EventList.Helpers;
using EventList.Pages.Events;
using EventList.Pages.Sessions;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.Favorites
{
    public partial class FavoritesPage : ContentPage
    {
		FavoriteViewModel vm;
		FavoriteViewModel ViewModel => vm ?? (vm = BindingContext as FavoriteViewModel);
        public FavoritesPage()
        {
            InitializeComponent();
			BindingContext = new FavoriteViewModel(Navigation);
			ListViewFavorites.ItemTapped += (sender, e) => ListViewFavorites.SelectedItem = null;
			ListViewFavorites.ItemSelected += async (sender, e) =>
			{
				FavoriteItemsViewModel ev = ListViewFavorites.SelectedItem as FavoriteItemsViewModel;

				if (ev == null)
					return;

                if (ev.FavoriteSession == null)
                {
                    var eventDetails = new EventDetailsPage();

                    eventDetails.Event = ev.FavoriteEvent;

                    await this.Navigation.PushAsync(eventDetails);
                }
                if (ev.FavoriteEvent == null)
				{
					var sessionDetails = new SessionDetailsPage(ev.FavoriteSession);
					await NavigationService.PushAsync(Navigation, sessionDetails);
				}
				ListViewFavorites.SelectedItem = null;
			};
        }
		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.LoadFavoritesCommand.Execute(null);
		}
    }
}
