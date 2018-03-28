using System;
namespace EventList
{
	public class SponsorLevel : Realms.RealmObject
	{
		public string Id { get; set; }

		public string RemoteId { get; set; }

		public string SponserId { get; set; }

		public string Name { get; set; }

		public int Rank { get; set; }
	}
}