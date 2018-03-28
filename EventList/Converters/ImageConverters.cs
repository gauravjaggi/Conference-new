using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace EventList
{
	class IsFilledIconConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
		(bool)value ? $"{parameter}_filled.png" : $"{parameter}_empty.png";

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class SpeakerImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				if (!string.IsNullOrWhiteSpace((string)value))
				{
					return new UriImageSource
					{
						Uri = new Uri((string)value),
						CachingEnabled = true,
						CacheValidity = TimeSpan.FromDays(3)
					};
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to convert image to URI: " + ex);
			}
			return ImageSource.FromFile("profile_generic_big.png");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}

