﻿using System;
using Xamarin.Forms.Platform.Android;
using Android.Support.Design.Widget;
using Android.Runtime;
using Xamarin.Forms;
using EventList.Droid;
using Android.Widget;
using EventList.Droid.Renderers;
using Android.Views;
using EventList.Models;

[assembly: ExportRenderer(typeof(EventList.NavigationView), typeof(NavigationViewRenderer))]
namespace EventList.Droid.Renderers
{
    public class NavigationViewRenderer : ViewRenderer<EventList.NavigationView,Android.Support.Design.Widget.NavigationView>
	{
		Android.Support.Design.Widget.NavigationView navView;
		ImageView profileImage;
		TextView profileName;
		protected override void OnElementChanged(ElementChangedEventArgs<EventList.NavigationView> e)
		{

			base.OnElementChanged(e);
			if (e.OldElement != null || Element == null)
				return;


			var view = Inflate(Forms.Context, Resource.Layout.nav_view, null);
			navView = view.JavaCast<Android.Support.Design.Widget.NavigationView>();


			navView.NavigationItemSelected += NavView_NavigationItemSelected;

			Settings.Current.PropertyChanged += SettingsPropertyChanged;
			SetNativeControl(navView);

			var header = navView.GetHeaderView(0);
			//profileImage = header.FindViewById<ImageView>(Resource.Id.profile_image);
			//profileName = header.FindViewById<TextView>(Resource.Id.profile_name);

			//profileImage.Click += (sender, e2) => NavigateToLogin();
			//profileName.Click += (sender, e2) => NavigateToLogin();

			UpdateName();
			UpdateImage();

			navView.SetCheckedItem(Resource.Id.nav_feed);
		}

		void NavigateToLogin()
		{
			if (Settings.Current.IsLoggedIn)
				return;
		}

		void SettingsPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Settings.Current.Email))
			{
				UpdateName();
				UpdateImage();
			}
		}

		void UpdateName()
		{
			//profileName.Text = Settings.Current.UserDisplayName;
		}

		void UpdateImage()
		{
			//Koush.UrlImageViewHelper.SetUrlDrawable(profileImage, Settings.Current.UserAvatar, Resource.Drawable.profile_generic);
		}

		public override void OnViewRemoved(Android.Views.View child)
		{
			base.OnViewRemoved(child);
			navView.NavigationItemSelected -= NavView_NavigationItemSelected;
			Settings.Current.PropertyChanged -= SettingsPropertyChanged;
		}

		IMenuItem previousItem;

		void NavView_NavigationItemSelected(object sender, Android.Support.Design.Widget.NavigationView.NavigationItemSelectedEventArgs e)
		{


			if (previousItem != null)
				previousItem.SetChecked(false);

			navView.SetCheckedItem(e.MenuItem.ItemId);

			previousItem = e.MenuItem;

			int id = 0;
			switch (e.MenuItem.ItemId)
			{
				case Resource.Id.nav_feed:
					id = (int)AppPage.Feed;
					break;
				case Resource.Id.nav_sessions:
					id = (int)AppPage.Sessions;
					break;
				case Resource.Id.nav_events:
					id = (int)AppPage.Events;
					break;		
                case Resource.Id.nav_mini_hacks:
                    id = (int)AppPage.MiniHacks;
                    break;
			}
			this.Element.OnNavigationItemSelected(new NavigationItemSelectedEventArgs
			{

				Index = id
			});
		}


	}
}

