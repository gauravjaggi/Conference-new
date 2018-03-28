using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.Pages.Speakers;
using Xamarin.Forms;

namespace EventList
{
	public class SpeakerCell : ViewCell
	{
		readonly INavigation navigation;
		string sessionId;
		public SpeakerCell(string sessionId, INavigation navigation = null)
		{
			this.sessionId = sessionId;
			Height = 60;
			View = new SpeakerCellView();
			StyleId = "disclosure";
			this.navigation = navigation;
		}

		protected override async void OnTapped()
		{
			base.OnTapped();
			if (navigation == null)
				return;

			var speaker = BindingContext as Speaker;
			if (speaker == null)
				return;
            

			//await navigation.PushAsync(new SpeakerDetailsPage(sessionId)
			//{
			//	Speaker = speaker
			//});
		}
	}
	public partial class SpeakerCellView : ContentView
	{
		public SpeakerCellView()
		{
			InitializeComponent();
		}
	}
}

