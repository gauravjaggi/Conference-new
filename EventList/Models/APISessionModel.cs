using System;
using Newtonsoft.Json;
namespace EventList.Models
{
    public class APISessionModel
    {
        [JsonProperty(PropertyName = "sessionid")]
        public string SessionIid { get; set; }

		[JsonProperty(PropertyName = "remoteid")]
		public string RemoteId { get; set; }

		[JsonProperty(PropertyName = "title")]
		public string Title { get; set; }

		[JsonProperty(PropertyName = "shorttitle")]
		public string ShortTitle { get; set; }

		[JsonProperty(PropertyName = "abstract")]
		public string Abstract { get; set; }

		[JsonProperty(PropertyName = "starttime")]
        public DateTime StartTime { get; set; }

        [JsonProperty(PropertyName = "endtime")]
		public DateTime EndTime { get; set; }

		[JsonProperty(PropertyName = "roomid")]
		public string RoomId { get; set; }

		[JsonProperty(PropertyName = "roomname")]
		public string RoomName { get; set; }

		[JsonProperty(PropertyName = "imageurl")]
		public string ImageUrl { get; set; }

		[JsonProperty(PropertyName = "latitude")]
		public double Latitude { get; set; }

		[JsonProperty(PropertyName = "longitude")]
		public double Longitude { get; set; }

		[JsonProperty(PropertyName = "categoryid")]
		public string CategoryId { get; set; }

		[JsonProperty(PropertyName = "categoryname")]
		public string CategoryName { get; set; }

        [JsonProperty(PropertyName = "shortname")]
        public string ShortName { get; set; }

		[JsonProperty(PropertyName = "color")]
		public string Color { get; set; }

		[JsonProperty(PropertyName = "speakerid")]
		public string SpeakerId { get; set; }

		[JsonProperty(PropertyName = "firstname")]
		public string FirstName { get; set; }

		[JsonProperty(PropertyName = "lastname")]
		public string LastName { get; set; }

		[JsonProperty(PropertyName = "biography")]
		public string Biography { get; set; }

		[JsonProperty(PropertyName = "photourl")]
		public string PhotoUrl { get; set; }

		[JsonProperty(PropertyName = "avatarurl")]
		public string AvatarUrl { get; set; }

		[JsonProperty(PropertyName = "positionname")]
		public string PositionName { get; set; }

		[JsonProperty(PropertyName = "companyname")]
		public string CompanyName { get; set; }

		[JsonProperty(PropertyName = "companywebsiteurl")]
		public string CompanyWebsiteUrl { get; set; }

		[JsonProperty(PropertyName = "blogurl")]
		public string BlogUrl { get; set; }

		[JsonProperty(PropertyName = "twitterurl")]
		public string TwitterUrl { get; set; }

		[JsonProperty(PropertyName = "linkdinurl")]
		public string LinkdinUrl { get; set; }

		[JsonProperty(PropertyName = "email")]
		public string Email { get; set; }
		
    }
}
