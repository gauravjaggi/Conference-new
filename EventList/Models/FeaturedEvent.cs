using System;
using Newtonsoft.Json;

namespace EventList
{
	public class FeaturedEvent :Realms.RealmObject
	{
		public string Id { get; set; }

		public string RemoteId { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public DateTimeOffset? StartTime { get; set; }

		public DateTimeOffset? EndTime { get; set; }

		public bool IsAllDay { get; set; }

		public string LocationName { get; set; }

		public Sponsor Sponsor { get; set; }

		//[JsonIgnore]
		[Realms.Ignored]
		public bool HasSponsor => Sponsor != null;

    	//[JsonIgnore]
		[Realms.Ignored]
		public DateTimeOffset StartTimeOrderBy { get { return StartTime.HasValue ? StartTime.Value : DateTime.MinValue; } }

		bool isFavorite;
		[Realms.Ignored]
		public bool IsFavorite
		{
			get { return isFavorite; }
			set
			{
				isFavorite = value;
				RaisePropertyChanged("IsFavorite");
			}
		}
		bool feedbackLeft;
		[Realms.Ignored]
		public bool FeedbackLeft
		{
			get { return feedbackLeft; }
			set
			{
				feedbackLeft = value;
				RaisePropertyChanged("FeedbackLeft");
				//SetProperty(ref feedbackLeft, value);
			}
		}

	}
}

