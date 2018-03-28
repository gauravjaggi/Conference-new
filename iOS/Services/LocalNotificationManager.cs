using System;
using EventList.Helpers;
using EventList.iOS;
using EventList.Models;
using Foundation;
using UIKit;
using UserNotifications;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationManager))]
namespace EventList.iOS
{
	public class LocalNotificationManager:UserNotificationCenterDelegate, INotificationManager
	{
		public const string eventNotifID = "cat1";
		public const string sessionNotifID = "cat2";
		public LocalNotificationManager()
		{
		}

		public void CreateEventNotication(FeaturedEvent featuredevent)
		{
         	DateTime eventDate = ((DateTime)featuredevent.StartTime.Value.UtcDateTime).ToLocalTime();
			UNMutableNotificationContent content = new UNMutableNotificationContent();
			content.Title = featuredevent.Title;
			content.Subtitle = string.Concat(eventDate.ToString("dd-MM-yyyy"));

			content.Body = " ";
			if (!string.IsNullOrEmpty(featuredevent.Description))
			{
				content.Body = featuredevent.Description;
			}
			content.CategoryIdentifier = eventNotifID;
				
			eventDate = eventDate.AddMinutes(-10);

			UNCalendarNotificationTrigger trigger = UNCalendarNotificationTrigger.CreateTrigger(eventDate.ToNSDateComponents(), true);

			UNNotificationRequest notification = UNNotificationRequest.FromIdentifier(featuredevent.Id, content, null);
			UNUserNotificationCenter.Current.AddNotificationRequest(notification, (NSError error) =>
			{
				if (error != null)
				{
					Console.WriteLine(error);
				}
			});

		}

		public void CreateSessionNotication(Session session)
		{
			DateTime sessionDate = ((DateTime)session.StartTime.Value.UtcDateTime).ToLocalTime();
			UNMutableNotificationContent content = new UNMutableNotificationContent();
			content.Title = session.Title;
			content.Subtitle = string.Concat(sessionDate.ToString("dd-MM-yyyy"));

			content.Body = " ";

			content.CategoryIdentifier = sessionNotifID;
	
			sessionDate = sessionDate.AddMinutes(-10);

            UNCalendarNotificationTrigger trigger = UNCalendarNotificationTrigger.CreateTrigger(DateTime.Now.AddMinutes(-10).ToNSDateComponents(), false);//sessionDate.ToNSDateComponents(), false);

			UNNotificationRequest notification = UNNotificationRequest.FromIdentifier(session.Id, content, trigger);
			UNUserNotificationCenter.Current.AddNotificationRequest(notification, (NSError error) =>
			{
				if (error != null)
				{
					Console.WriteLine(error);
				}
			});

		}

        public void DeleteEventNotification(FeaturedEvent featuredevent)
        {
            
        }

        public void DeleteSessionNotification(Session session)
        {
            
        }
    }
}
