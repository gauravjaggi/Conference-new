using System;
using EventList.Models;

namespace EventList.Services
{
    public interface IAndroidReminderService
    {
        void RemindEvent(FeaturedEventDataModel featuredevent);

        void RemindSession(SessionDataModel session);
    }
}
