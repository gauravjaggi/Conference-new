using System;
using Android.App;
using Android.Content;
using Android.OS;
using EventList.Droid.Notifications;
using EventList.Droid.Services;
using EventList.Helpers;
using EventList.Models;
using Java.Util;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocalNotificationManager))]
namespace EventList.Droid.Services
{
    public class LocalNotificationManager : INotificationManager
    {
        PendingIntent pendingIntent;
        public LocalNotificationManager()
        {

        }

        public void CreateEventNotication(FeaturedEvent featuredevent) 
        {
            DateTime eventDate = featuredevent.StartTime.Value.DateTime;
			Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
			alarmIntent.PutExtra("message", featuredevent.Description);
			alarmIntent.PutExtra("title", featuredevent.Title);

			pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
                      
         	//DateTime when =eventDate.AddMinutes(-10);
			DateTime when = DateTime.Now.AddMinutes(2); //for testing purpose

            int secs = (when - DateTime.Now).Seconds;
            alarmManager.Set(AlarmType.ElapsedRealtime,SystemClock.ElapsedRealtime() + secs * 1000, pendingIntent);

		}

        public void CreateSessionNotication(Session session)
        {
            DateTime sessionDate = session.StartTime.Value.DateTime;
            Intent alarmIntent = new Intent(Forms.Context, typeof(AlarmReceiver));
            alarmIntent.PutExtra("id", session.Id);
            alarmIntent.PutExtra("message", session.ShortTitle);
            alarmIntent.PutExtra("title", session.Title);

            pendingIntent = PendingIntent.GetBroadcast(Forms.Context, 0, alarmIntent, PendingIntentFlags.UpdateCurrent);
            AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);

            DateTime when = sessionDate.AddMinutes(-10);
            //DateTime when = DateTime.Now.AddMinutes(2); //for testing purpose

            int secs = (when - DateTime.Now).Seconds;
            alarmManager.Set(AlarmType.ElapsedRealtime, SystemClock.ElapsedRealtime() + secs * 1000, pendingIntent);

        }

        public void DeleteEventNotification(FeaturedEvent featuredevent)
        {
            Intent intent = new Intent(Forms.Context,typeof(AlarmReceiver));
			PendingIntent sender = PendingIntent.GetBroadcast(Forms.Context, 0, intent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
			alarmManager.Cancel(sender);
        }

        public void DeleteSessionNotification(Session session)
        {
			Intent intent = new Intent(Forms.Context, typeof(AlarmReceiver));
			PendingIntent sender = PendingIntent.GetBroadcast(Forms.Context, 0, intent, PendingIntentFlags.UpdateCurrent);
			AlarmManager alarmManager = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
			alarmManager.Cancel(sender);
        }
    }

}
