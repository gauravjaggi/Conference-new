using System;
using MvvmHelpers;

namespace EventList
{
	public interface IBaseDataObject
	{
		string Id { get; set; }
	}
#if BACKEND
    public class BaseDataObject : EntityData
    {
        public BaseDataObject ()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string RemoteId { get; set; }
    }
#else
public class BaseDataObject : ObservableObject, IBaseDataObject
{
	public BaseDataObject()
	{
		Id = Guid.NewGuid().ToString();
	}

	public string RemoteId { get; set; }

	[Newtonsoft.Json.JsonProperty("Id")]
	public string Id { get; set; }

}
#endif
}
