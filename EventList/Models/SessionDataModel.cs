using System;
using System.Collections.Generic;

namespace EventList.Models
{
    public class SessionDataModel
    {
		public string Id { get; set; }

		public string RemoteId { get; set; }
		
		public string Title { get; set; }
        		
		public string ShortTitle { get; set; }
        		
		public string Abstract { get; set; }

		public List<Speaker> Speakers { get; }

		public Room Room { get; set; }

		public Category MainCategory { get; set; }

		public DateTime StartTime { get; set; }
	
		public DateTime EndTime { get; set; }

		public List<Speaker> SessionSpeakers { get; set; }
    }
}
