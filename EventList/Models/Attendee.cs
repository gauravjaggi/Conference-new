using System;
namespace EventList.Models
{
    public class Attendee:Realms.RealmObject
    {
		public string AttendeeId { get; set; }

		public string RemoteId { get; set; }

		public string SessionId { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Biography { get; set; }

		public string PhotoUrl { get; set; }

		public string AvatarUrl { get; set; }

		public string PositionName { get; set; }

		public string CompanyName { get; set; }

		public string CompanyWebsiteUrl { get; set; }

		public string BlogUrl { get; set; }

		public string TwitterUrl { get; set; }

		public string LinkedInUrl { get; set; }

		public string Email { get; set; }

		[Realms.Ignored]
		public string FullName { get { return $"{FirstName} {LastName}"; } }

		[Realms.Ignored]
		public string Title
		{
			get
			{
				if (string.IsNullOrWhiteSpace(CompanyName))
					return PositionName;

				return $"{PositionName}, {CompanyName}";
			}
		}

		[Realms.Ignored]
		public Uri AvatarUri
		{
			get
			{
				try
				{
					return new Uri(AvatarUrl);
				}
				catch
				{

				}
				return null;
			}
		}
		[Realms.Ignored]
		public Uri PhotoUri
		{
			get
			{
				try
				{
					return new Uri(PhotoUrl);
				}
				catch
				{

				}
				return null;
			}
		}
    }
}
