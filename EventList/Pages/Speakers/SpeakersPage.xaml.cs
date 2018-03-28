using System;
using System.Collections.Generic;
using EventList.Models;
using Xamarin.Forms;

namespace EventList.Pages.Speakers
{
    public partial class SpeakersPage : ContentPage
    {
        SpeakerViewModel vm;
        public SpeakersPage()
        {
            InitializeComponent();
            BindingContext = vm = new SpeakerViewModel(Navigation);
            ListViewSpeakers.ItemTapped += (sender, e) => ListViewSpeakers.SelectedItem = null;
			ListViewSpeakers.ItemSelected += async (sender, e) =>
			{

				Speaker ev = ListViewSpeakers.SelectedItem as Speaker;

				if (ev == null)
					return;

                var speakerDetails = new SpeakerDetailsPage();

                speakerDetails.Speaker = ev;
				await this.Navigation.PushAsync(speakerDetails);

				ListViewSpeakers.SelectedItem = null;
			};
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadSpeakersCommand.Execute(null);
        }
    }
}
