using System;
using Foundation;

namespace EventList.iOS
{
	public static class DateExtensions
	{
		public static NSDateComponents ToNSDateComponents(this DateTime date)
		{

			return new NSDateComponents()
			{
				Year = date.Year,
				Minute = date.Minute,
				Second = date.Second,
				Hour = date.Hour,
				Month = date.Month,
				Day = date.Day
			};   
		}
	}
}
