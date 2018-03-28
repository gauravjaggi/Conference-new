using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace EventList
{
    public partial class FeedsLabelSection : ContentView
    {
        public FeedsLabelSection()
        {
            InitializeComponent();
        }
		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(nameof(Text), typeof(string), typeof(FeedsLabelSection), string.Empty);

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);
			if (propertyName == TextProperty.PropertyName)
			{
				Section.Text = Device.OS == TargetPlatform.iOS ? Text.ToUpperInvariant() : Text;
			}
		}
    }
}
