using System;
using System.Collections.Generic;
using EventList.Helpers;
using EventList.Pages.Events;
using EventList.Pages.Sessions;
using Xamarin.Forms;

namespace EventList.Pages.Schedule
{
    public partial class SchedulePage : ContentPage
    {
		ScheduleViewModel vm;
		ScheduleViewModel ViewModel => vm ?? (vm = BindingContext as ScheduleViewModel);
        public SchedulePage()
        {
            InitializeComponent();
            BindingContext = new ScheduleViewModel(Navigation);
			lvSchedules.ItemTapped += (sender, e) => lvSchedules.SelectedItem = null;
			lvSchedules.ItemSelected += async (sender, e) =>
			{
                ScheduleItemViewModel ev = lvSchedules.SelectedItem as ScheduleItemViewModel;

				if (ev == null)
					return;

                if (ev.ScheduledSession == null)
				{
					var eventDetails = new EventDetailsPage();

                    eventDetails.Event = ev.ScheduledEvent;

					await this.Navigation.PushAsync(eventDetails);
				}
				if (ev.ScheduledEvent == null)
				{
                    var sessionDetails = new SessionDetailsPage(ev.ScheduledSession);
					await NavigationService.PushAsync(Navigation, sessionDetails);
				}
				lvSchedules.SelectedItem = null;
			};
        }
		protected override void OnAppearing()
		{
			base.OnAppearing();
            ViewModel.LoadSchedulesCommand.Execute(null);
		}
    }
}
