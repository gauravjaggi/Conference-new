using System;
namespace EventList
{
	public static class Constants
	{
		public static string SyncHost { get; set; } = "52.53.230.135:9080";


		public static string Username { get; } = "fish@gmail.com";

		public static string Password { get; } = "asd@123";

		public static Uri SyncServerUri => new Uri($"realm://{SyncHost}/~/ConferenceSessions23");

		public static Uri AuthServerUri => new Uri($"https://{SyncHost}");

		public static Uri ChatSyncServerUri => new Uri($"realm://{SyncHost}/~/Chat23");

	}
}
