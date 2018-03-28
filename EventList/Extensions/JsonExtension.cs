using System;
namespace EventList
{
    public static class JsonExtension
    {
		#region Json Serializer
		public static string ToJson(this object obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		}

		public static T FromJson<T>(this string jsonData)
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
		}
		#endregion
	}
}
