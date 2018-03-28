using System;
namespace EventList.Models
{
	//public class Favorite : BaseDataObject
	//{
	//	public string UserId { get; set; }
	//	public string SessionId { get; set; }
	//}
    public class Favorite : Realms.RealmObject
	{
        public string Id { get; set; }

		public string DeviceUser { get; set; }
		
        public string EventType { get; set; }

	}
}