using System;
using System.Collections.Generic;
using EventList.Models;
using Xamarin.Forms;

namespace EventList.Pages.Speakers
{
    public partial class AttendeeDetailPage : ContentPage
    {
        AttendeesDetailViewModel ViewModel => vm ?? (vm = BindingContext as AttendeesDetailViewModel);
		AttendeesDetailViewModel vm;
        public AttendeeDetailPage()
        {
            InitializeComponent();
			MainScroll.ParallaxView = HeaderView;

			if (Device.Idiom != TargetIdiom.Phone)
				Row1Header.Height = Row1Content.Height = 350;
        }

		public Attendee Attendee
		{
            get { return ViewModel.Attendee; }
			set { BindingContext = new AttendeesDetailViewModel(value); }
		}
    }
}
