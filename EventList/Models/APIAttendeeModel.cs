using System;
using Newtonsoft.Json;

namespace EventList.Models
{
    public class APIAttendeeModel
    {
        [JsonProperty(PropertyName = "attendeeid")]
        public string AttendeeId { get; set; }

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

		[JsonProperty(PropertyName = "sessionid")]
		public string SessionId { get; set; }

		[JsonProperty(PropertyName = "remoteid")]
		public string RemoteId { get; set; }
    }
}
