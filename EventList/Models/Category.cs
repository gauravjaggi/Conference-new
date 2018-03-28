using System;
using Newtonsoft.Json;
namespace EventList.Models
{
	public class Category : Realms.RealmObject
	{


		public string Id { get; set; }

		public string RemoteId { get; set; }
		public string SessionId { get; set; }

		
		public string Name { get; set; }


		public string ShortName { get; set; }

		public string Color { get; set; }

		
		[Realms.Ignored]
		public string BadgeName => string.IsNullOrWhiteSpace(ShortName) ? Name : ShortName;
#if MOBILE
        bool filtered;
        //[JsonIgnore]
		[Realms.Ignored]
        public bool IsFiltered
        {
            get { return filtered; }
            set { 
					//SetProperty(ref filtered, value);
					filtered = value;
					RaisePropertyChanged("IsFiltered");
				}
        }

        bool enabled;
        //[JsonIgnore]
		[Realms.Ignored]
        public bool IsEnabled
        {
            get { return enabled; }
            set {
				//SetProperty(ref enabled, value); 
		 		enabled = value;
				RaisePropertyChanged("IsEnabled");
			}
        }
       
#endif
	}
}