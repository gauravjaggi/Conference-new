using System;
using System.Collections.Generic;
using EventList.Interfaces;
using Xamarin.Forms;

namespace EventList
{
	public class EvolveMap
	{
		public string Local { get; set; }
		public string Url { get; set; }
		public string Title { get; set; }
	}
    public partial class FloorMap : ContentView
    {
		public FloorMap()
		{
			InitializeComponent();

			BindingContextChanged += FloorMap_BindingContextChanged;


			MainImage.PropertyChanged += (sender, e) =>
			{
				if (e.PropertyName != nameof(MainImage.IsLoading))
					return;
				ProgressBar.IsRunning = MainImage.IsLoading;
				ProgressBar.IsVisible = MainImage.IsLoading;
			};
		}

		private void FloorMap_BindingContextChanged(object sender, EventArgs e)
		{
			var data = BindingContext as EvolveMap;

			try
			{
				MainImage.Source = new UriImageSource
				{
					Uri = new Uri(data.Url),
					CachingEnabled = true,
					CacheValidity = TimeSpan.FromDays(3)
				};
			}
			catch (Exception ex)
			{
				DependencyService.Get<IToast>().SendToast("Unable to load image.");

			}
		}
    }
}
