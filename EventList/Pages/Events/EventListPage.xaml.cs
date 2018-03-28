using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace EventList.Pages.Events
{
	public partial class EventListPage : ContentPage
	{
		EventsViewModel vm;
		EventsViewModel ViewModel => vm ?? (vm = BindingContext as EventsViewModel);

		public EventListPage()
		{
			InitializeComponent();
			BindingContext = new EventsViewModel(Navigation);

			if (Device.OS == TargetPlatform.Windows || Device.OS == TargetPlatform.WinPhone)
			{
				ToolbarItems.Add(new ToolbarItem
				{
					Text = "Refresh",
					Command = ViewModel.ForceRefreshCommand
				});
			}

			ListViewEvents.ItemTapped += (sender, e) => ListViewEvents.SelectedItem = null;
			ListViewEvents.ItemSelected += async (sender, e) =>
			{
                
				FeaturedEvent ev = ListViewEvents.SelectedItem as FeaturedEvent;
               
				if (ev == null)
					return;

				var eventDetails = new EventDetailsPage();

				eventDetails.Event = ev;
				await this.Navigation.PushAsync(eventDetails);

				ListViewEvents.SelectedItem = null;
			};
		}
		public void OnSearchClicked(object sender, EventArgs e)
		{
			searchbar.IsVisible = true;
			searchbar.Focus();
		}
		void Handle_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			vm.SearchEventsCommand.Execute(null);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			searchbar.IsVisible = false;
			if (ViewModel.Events.Count == 0)
				ViewModel.LoadEventsCommand.Execute(false);
		}

		protected override void OnDisappearing()
		{

			base.OnDisappearing();
		}
	}
}//ImageCircle.Forms.Plugin.Abstractions

