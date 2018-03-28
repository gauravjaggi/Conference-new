using System;
using UserNotifications;

namespace EventList.iOS
{
	public class UserNotificationCenterDelegate : UNUserNotificationCenterDelegate
	{
		public override void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
		{
			completionHandler.Invoke(UNNotificationPresentationOptions.Alert);
		}
		public override void DidReceiveNotificationResponse(UNUserNotificationCenter center, UNNotificationResponse response, Action completionHandler)
		{
			var catid = response.Notification.Request.Content.CategoryIdentifier;
			switch (catid)
			{ 
				case "cat1":
						break;
				case "cat2":
					break;
			}
		}
	}
}
