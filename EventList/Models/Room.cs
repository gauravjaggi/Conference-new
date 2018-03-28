using System;
using Newtonsoft.Json;
namespace EventList.Models
{
	public class Room : Realms.RealmObject
	{
		public string Id { get; set; }

		public string RemoteId { get; set; }
		public string SessionId { get; set; }

		public string Name { get; set; }
	
		public string ImageUrl { get; set; }

		public double? Latitude { get; set; }

		public double? Longitude { get; set; }

#if MOBILE
        //[Newtonsoft.Json.JsonIgnore]
		[Realms.Ignored]
        public Uri ImageUri 
        { 
            get 
            { 
                try
                {
                    return new Uri(ImageUrl);
                }
                catch
                {

                }
                return null;
            } 
        }
#endif
	}
}