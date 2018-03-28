using System;
using EventList.Models;

namespace EventList
{
    public class ScheduleItemViewModel:ViewModelBase
    {
		public Session ScheduledSession { get; set; }

		public FeaturedEvent ScheduledEvent { get; set; }
    }
}
