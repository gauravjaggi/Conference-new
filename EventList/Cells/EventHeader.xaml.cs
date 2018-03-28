using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EventList
{
	public class EventGroupHeader : ViewCell
	{
		public EventGroupHeader()
		{
			View = new EventHeader();
		}
	}
	
	public partial class EventHeader : ContentView
	{
		public EventHeader()
		{
			InitializeComponent();
		}
	}
}
