using System;
namespace EventList.Models
{
    public class Feedback : Realms.RealmObject
	{

        public string Username { get; set; }
		public string EventType { get; set; }
		public string EventID { get; set; }

		public int ContentRating { get; set; }
		public int PresentationRating { get; set; }
		public int PublicSpeakingRating { get; set; }	
		public int WorkShopRating { get; set; }

		public bool IsContentRelevant { get; set; }
		public bool IsGoodToRecommend { get; set; }

		public string Comments { get; set; }
		public string Notes { get; set; }
	}
}