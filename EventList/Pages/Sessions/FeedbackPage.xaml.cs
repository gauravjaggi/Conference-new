﻿using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.Sessions
{
	public partial class FeedbackPage : ContentPage
	{
		FeedbackViewModel vm;
		public FeedbackPage()
		{
			InitializeComponent();

			if (Device.OS != TargetPlatform.iOS)
				ToolbarDone.Icon = "toolbar_close.png";


			ToolbarDone.Command = new Command(async () =>
				{
					if (vm.IsBusy)
						return;

					await Navigation.PopModalAsync();
				});
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			var items = StarGrid.Behaviors.Count;
			for (int i = 0; i < items; i++)
				StarGrid.Behaviors.RemoveAt(i);
		}
	}
}

