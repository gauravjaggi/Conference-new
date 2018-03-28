using System;
namespace EventList.Models
{
    public class FeaturedEventDataModel
    {
		public string Id { get; set; }

		public string RemoteId { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTime StartTime { get; set; }

		public DateTime EndTime { get; set; }

		public bool IsAllDay { get; set; }

		public string LocationName { get; set; }

		public Sponsor Sponsor { get; set; }

		//[Newtonsoft.Json.JsonIgnore]
		//public bool HasSponsor => Sponsor != null;

		//[Newtonsoft.Json.JsonIgnore]
		//public DateTime StartTimeOrderBy { get { return StartTime. ? StartTime : DateTime.MinValue; } }
    }
}
