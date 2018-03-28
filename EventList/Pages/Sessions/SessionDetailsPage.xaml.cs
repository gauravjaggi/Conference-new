using System;
using System.Collections.Generic;
using EventList.Helpers;
using EventList.Models;
using EventList.Pages.Feedback;
using EventList.Pages.Speakers;
using EventList.ViewModels;
using FormsToolkit;
using Xamarin.Forms;

namespace EventList.Pages.Sessions
{
	public partial class SessionDetailsPage : ContentPage
	{
		SessionDetailsViewModel ViewModel => vm ?? (vm = BindingContext as SessionDetailsViewModel);
		SessionDetailsViewModel vm;
		public SessionDetailsPage(Session session)
		{
			InitializeComponent();

            //ListViewSpeakers.ItemSelected += async (sender, e) =>
            //{
            //	var speaker = ListViewSpeakers.SelectedItem as Speaker;
            //	if (speaker == null)
            //		return;

            //	//var speakerDetails = new SpeakerDetailsPage(vm.Session.Id);

            //	speakerDetails.Speaker = speaker;
            //	await NavigationService.PushAsync(Navigation, speakerDetails);
            //	ListViewSpeakers.SelectedItem = null;
            //};


            //ButtonRate.Clicked += async (sender, e) =>
            //{
            //             await NavigationService.PushAsync(Navigation, new RatingPage(session, null, false));
            //};

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += async (sender, e) =>
            {
                await NavigationService.PushAsync(Navigation, new RatingPage(session, null, false));
            };
            ButtonRate.GestureRecognizers.Add(tap);

            BindingContext = new SessionDetailsViewModel(Navigation, session);
			ViewModel.LoadSessionCommand.Execute(null);

		}

        void ListViewTapped(object sender, ItemTappedEventArgs e)
		{
			var list = sender as ListView;
			if (list == null)
				return;
			list.SelectedItem = null;
		}



		void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
		{
			if (e.ScrollY > SessionDate.Y)
				Title = ViewModel.Session.ShortTitle;
			else
				Title = "Session Details";
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			MainScroll.Scrolled += MainScroll_Scrolled;
			ListViewSpeakers.ItemTapped += ListViewTapped;

            if (Device.OS == TargetPlatform.Android || Device.OS == TargetPlatform.iOS)
            {
                //Application.Current.AppLinks.RegisterLink(ViewModel.Session.GetAppLink());
            }
			var count = ViewModel?.Session?.Speakers?.Count ?? 0;
			var adjust = Device.OS != TargetPlatform.Android ? 1 : -count + 1;
			if ((ViewModel?.Session?.Speakers?.Count ?? 0) > 0)
				ListViewSpeakers.HeightRequest = (count * ListViewSpeakers.RowHeight) - adjust;

		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			MainScroll.Scrolled -= MainScroll_Scrolled;
			ListViewSpeakers.ItemTapped -= ListViewTapped;
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			vm = null;

			ListViewSpeakers.HeightRequest = 0;



		}
	}
}

