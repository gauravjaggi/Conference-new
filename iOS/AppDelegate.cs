﻿using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin;
using System.Threading.Tasks;
using EventList.iOS.Renderers;
using Refractored.XamForms.PullToRefresh.iOS;
using FormsToolkit.iOS;
using UserNotifications;
using Google.Core;
using Google.SignIn;
//using CarouselView.FormsPlugin.iOS;

namespace EventList.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{

            var dictionary = NSDictionary.FromObjectsAndKeys(new[] { "Mozilla/5.0 (iPhone; CPU iPhone OS 7_1 like Mac OS X) AppleWebKit/537.51.2 (KHTML, like Gecko) Version/7.0 Mobile/11D167 Safari/9537.53" }, new[] { "UserAgent" });
            NSUserDefaults.StandardUserDefaults.RegisterDefaults(dictionary);

			var tint = UIColor.FromRGB(118, 53, 235);
			UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(250, 250, 250); //bar background
			UINavigationBar.Appearance.TintColor = tint; //Tint color of button items

			UIBarButtonItem.Appearance.TintColor = tint; //Tint color of button items

			UITabBar.Appearance.TintColor = tint;

			UISwitch.Appearance.OnTintColor = tint;

			UIAlertView.Appearance.TintColor = tint;

			UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = tint;
			UIView.AppearanceWhenContainedIn(typeof(UIActivityViewController)).TintColor = tint;
            global::ZXing.Net.Mobile.Forms.iOS.Platform.Init();

			global::Xamarin.Forms.Forms.Init();
            //CarouselViewRenderer.Init();
			FormsMaps.Init();
            Toolkit.Init();
           // KeyboardOverlap.Forms.Plugin.iOSUnified.KeyboardOverlapRenderer.Init();
            NSError configureError;
            Context.SharedInstance.Configure(out configureError);
            if (configureError != null)
            {
                Console.WriteLine("Error configuring the Google context: {0}", configureError);
                Google.SignIn.SignIn.SharedInstance.ClientID = "510783581684-8232o8aai4s2uf5kkqnpusb0493aa3d1.apps.googleusercontent.com";
            }

           //Request for local notification
			UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
			{
				App.IsLocalNotifAllowed = approved;

			});
			UNUserNotificationCenter.Current.Delegate = new UserNotificationCenterDelegate();

			// Code for starting up the Xamarin Test Cloud Agent

			//Random Inits for Linking out.
			Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
			SQLitePCL.CurrentPlatform.Init();
			Plugin.Share.ShareImplementation.ExcludedUIActivityTypes = new List<NSString>
			{
				UIActivityType.PostToFacebook,
				UIActivityType.AssignToContact,
				UIActivityType.OpenInIBooks,
				UIActivityType.PostToVimeo,
				UIActivityType.PostToFlickr,
				UIActivityType.SaveToCameraRoll
			};
			//ImageCircle.Forms.Plugin.iOS.ImageCircleRenderer.Init();
			ZXing.Net.Mobile.Forms.iOS.Platform.Init();
			NonScrollableListViewRenderer.Initialize();
			SelectedTabPageRenderer.Initialize();
			TextViewValue1Renderer.Init();
			PullToRefreshLayoutRenderer.Init();

            LoadApplication(new App());
			return base.FinishedLaunching(app, options);
		}
		public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
		{
			var openUrlOptions = new UIApplicationOpenUrlOptions(options);
			return SignIn.SharedInstance.HandleUrl(url, openUrlOptions.SourceApplication, openUrlOptions.Annotation);
		}

        // For iOS 8 and older
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            return SignIn.SharedInstance.HandleUrl(url, sourceApplication, annotation);
        }
	}
}