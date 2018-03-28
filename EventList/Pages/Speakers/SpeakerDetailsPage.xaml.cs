using System;
using System.Collections.Generic;
using EventList.Helpers;
using EventList.Models;
using EventList.Pages.Sessions;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.Speakers
{
	public partial class SpeakerDetailsPage : ContentPage
	{
		SpeakerDetailsViewModel ViewModel => vm ?? (vm = BindingContext as SpeakerDetailsViewModel);
		SpeakerDetailsViewModel vm;
		string sessionId;
		public SpeakerDetailsPage()
		{
			InitializeComponent();
			MainScroll.ParallaxView = HeaderView;

			if (Device.Idiom != TargetIdiom.Phone)
				Row1Header.Height = Row1Content.Height = 350;

		}

		public Speaker Speaker
		{
			get { return ViewModel.Speaker; }
			set { BindingContext = new SpeakerDetailsViewModel(value); }
		}

		void MainScroll_Scrolled(object sender, ScrolledEventArgs e)
		{
			if (e.ScrollY > (MainStack.Height - SpeakerTitle.Height))
				Title = Speaker.FirstName;
			else
				Title = "Speaker Info";
		}

		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			MainScroll.Parallax();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			vm = null;

			var adjust = Device.OS != TargetPlatform.Android ? 1 : -ViewModel.FollowItems.Count + 2;
			ListViewFollow.HeightRequest = (ViewModel.FollowItems.Count * ListViewFollow.RowHeight) - adjust;
			
		}

		protected override async void OnAppearing()
		{

			base.OnAppearing();

			MainScroll.Scrolled += MainScroll_Scrolled;

			ListViewFollow.ItemTapped += ListViewTapped;
			
			MainScroll.Parallax();		
			
		}

		void ListViewTapped(object sender, ItemTappedEventArgs e)
		{
			var list = sender as ListView;
			if (list == null)
				return;
			list.SelectedItem = null;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			ListViewFollow.ItemTapped -= ListViewTapped;
			MainScroll.Scrolled -= MainScroll_Scrolled;
		}
	}
}

