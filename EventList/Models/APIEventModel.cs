using System;
using Newtonsoft.Json;

namespace EventList.Models
{
    public class APIEventModel
    {
        [JsonProperty(PropertyName = "eventid")]
		public string EventId { get; set; }

		[JsonProperty(PropertyName = "remoteid")]
		public string RemoteId { get; set; }

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "description")]
		public string Description { get; set; }

		[JsonProperty(PropertyName = "starttime")]
        public DateTime StartTime { get; set; }

		[JsonProperty(PropertyName = "endtime")]
		public DateTime EndTime { get; set; }

		[JsonProperty(PropertyName = "isallday")]
		public bool IsAllDay { get; set; }

		[JsonProperty(PropertyName = "locationname")]
		public string LocationName { get; set; }

        [JsonProperty(PropertyName = "sponsorid")]
		public string SponsorId { get; set; }

		[JsonProperty(PropertyName = "sponsorname")]
		public string SponsorName { get; set; }

        [JsonProperty(PropertyName = "sponsordescription")]
        public string SponsorDescription { get; set; }

		[JsonProperty(PropertyName = "imageurl")]
		public string ImageUrl { get; set; }

        [JsonProperty(PropertyName = "websiteurl")]
		public string WebsiteUrl { get; set; }

		[JsonProperty(PropertyName = "sessionid")]
		public string TwitterUrl { get; set; }

        [JsonProperty(PropertyName = "boothlocation")]
		public string BoothLocation { get; set; }

		[JsonProperty(PropertyName = "sponsorrank")]
		public int SponsorRank { get; set; }

		[JsonProperty(PropertyName = "sponsorlevelid")]
		public string SponsorLevelId { get; set; }

		[JsonProperty(PropertyName = "name")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "rank")]
		public int Rank { get; set; }
    }
}
