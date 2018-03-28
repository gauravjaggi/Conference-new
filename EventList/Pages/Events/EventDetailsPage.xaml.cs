using System;
using System.Collections.Generic;
using EventList.Helpers;
using Xamarin.Forms;

namespace EventList.Pages.Events
{
	public partial class EventDetailsPage : ContentPage
	{
		EventDetailsViewModel ViewModel => vm ?? (vm = BindingContext as EventDetailsViewModel);
		EventDetailsViewModel vm;
		public EventDetailsPage()
		{
			InitializeComponent();
			ButtonRate.Clicked += async (sender, e) =>
			{
                await NavigationService.PushAsync(Navigation, new Feedback.RatingPage(null,vm.Event, true));
			};
		}

		public FeaturedEvent Event
		{
			get { return ViewModel.Event; }
			set { BindingContext = new EventDetailsViewModel(Navigation, value); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			vm = null;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel.LoadEventDetailsCommand.Execute(null);
		}
	}
}