using System;
using System.Collections.Generic;
using EventList.Models;
using Xamarin.Forms;

namespace EventList.Pages.Speakers
{
    public partial class AttendeesPage : ContentPage
    {
        AttendeeViewModel vm;
        public AttendeesPage()
        {
            InitializeComponent();
			BindingContext = vm = new AttendeeViewModel(Navigation);
			ListViewSpeakers.ItemTapped += (sender, e) => ListViewSpeakers.SelectedItem = null;
			ListViewSpeakers.ItemSelected += async (sender, e) =>
			{

				Attendee ev = ListViewSpeakers.SelectedItem as Attendee;

				if (ev == null)
					return;

                var attendeeDetails = new AttendeeDetailPage();

                attendeeDetails.Attendee = ev;
				await this.Navigation.PushAsync(attendeeDetails);

				ListViewSpeakers.SelectedItem = null;
			};
        }
		protected override void OnAppearing()
		{
			base.OnAppearing();
			vm.LoadAttendeesCommand.Execute(null);
		}
    }
}
