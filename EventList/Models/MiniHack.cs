using System;
namespace EventList.Models
{
	public class MiniHack : Realms.RealmObject
	{
		public string Id { get; set; }

		public string RemoteId { get; set; }
		public string Name { get; set; }
		public string Subtitle { get; set; }
		public string Description { get; set; }
		public string GitHubUrl { get; set; }
		public string BadgeUrl { get; set; }
		public string UnlockCode { get; set; }

        //[Newtonsoft.Json.JsonIgnore]
		[Realms.Ignored]
        public Uri BadgeUri 
        { 
            get 
            { 
                try
                {
                    return new Uri(BadgeUrl);
                }
                catch
                {

                }
                return null;
            } 
        }

        bool isCompleted;
        //[Newtonsoft.Json.JsonIgnore]
		[Realms.Ignored]
        public bool IsCompleted
        {
            get { return isCompleted; }
            set {
				//SetProperty(ref isCompleted, value);
				isCompleted = value;
				RaisePropertyChanged("IsCompleted");
			}
        }

	}
}

