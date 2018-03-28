using System;
using Android.Widget;
using EventList.Droid.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(ListViewSelectionOnTopEffect), "ListViewSelectionOnTopEffect")]
namespace EventList.Droid.Helpers
{
	public class ListViewSelectionOnTopEffect : PlatformEffect
	{
		protected override void OnAttached()
		{
			try
			{
				var listView = Control as AbsListView;

				if (listView == null)
					return;

				listView.SetDrawSelectorOnTop(true);
			}
			catch (Exception ex)
			{

			}
		}

		protected override void OnDetached()
		{

		}
	}
}