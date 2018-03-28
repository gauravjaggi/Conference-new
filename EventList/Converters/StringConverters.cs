using System;
using System.Globalization;
using Xamarin.Forms;

namespace EventList
{
	class IsFavoriteTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;

			return (bool)value ? "Unfavorite" : "Favorite";
		}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
	class IsFavoriteDetailTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;

			return (bool)value ? "Remove from Favorites" : "Add to Favorites";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	class HasReminderEventTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;


			return (bool)value ? "Remove from Calendar" : "Add to Calendar";

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	class HasReminderTextConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
				return string.Empty;

			return (bool)value ? "Remove from Calendar" : "Add to Calendar";

		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
   
}

