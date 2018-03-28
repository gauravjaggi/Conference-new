using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace EventList
{
    public class Request
    {
       
		public static T Get<T>(WebRequest request)
		{
			string result = string.Empty;
            request.ContentType = "application/json";
       
			WebResponse webResponse = Task.Factory.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null).Result;
			using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
			{
				result = streamReader.ReadToEnd();
			}
			var typ = typeof(T);
			if (
				typ == typeof(String)
				|| typ == typeof(float)
				|| typ == typeof(Decimal)
				|| typ == typeof(Int16)
				|| typ == typeof(Int32)
				|| typ == typeof(Int64)
			)
			{
				return (T)Convert.ChangeType(result, typeof(T), null);
			}
			return result.FromJson<T>();
        }
	}
}
