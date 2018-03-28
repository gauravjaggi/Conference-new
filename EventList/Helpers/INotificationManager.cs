using System;
using EventList.Models;

namespace EventList.Helpers
{
    public interface INotificationManager
    {
        void CreateEventNotication(FeaturedEvent featuredevent);
        void CreateSessionNotication(Session session);
        void DeleteEventNotification(FeaturedEvent featuredevent);
		void DeleteSessionNotification(Session session);
    }
}