using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EventList.Models;

namespace EventList
{
    public static class APIServices
    {
        public const string BaseAPI = "http://52.52.29.58:8000/restapp/v1/q10?query={0}&format=json";
        public const string RealmSettingAPI = "http://52.52.29.58:8000/restapp/v1/q10?query=select%20*%20from%20appcodetable%20where%20code%20%3D%27{0}%27%20or%20code%20%3D%20%27default%27&format=json";
       
        public static Task<List<RealmSetting>> GetRealmSettings(string code)
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<RealmSetting>>(HttpWebRequest.Create(string.Format(RealmSettingAPI,code)));
                return res;
            });

        }
        public static Task<List<APISessionModel>> GetSession()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<APISessionModel>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20%20%09s.*%2C%20%20%20%20%20r.*%2C%20%20%20%20%20ca.*%2C%20%20%20%20%20spe.*%20from%20session%20as%20s%20join%20sessionroom%20as%20sr%20on%20s.sessionid%3Dsr.sessionid%20%20join%20sessioncategory%20as%20sc%20on%20s.sessionid%3Dsc.sessionid%20%20join%20speaker%20as%20sp%20on%20s.sessionid%3Dsp.sessionid%20left%20join%20room%20as%20r%20on%20r.roomid%3D%20sr.roomid%20left%20join%20category%20as%20ca%20on%20ca.categoryid%3D%20sc.categoryid%20left%20join%20speaker%20spe%20on%20spe.speakerid%3D%20sp.speakerid")));
                return res;
            });

        }

        public static Task<List<APIEventModel>> GetEvents()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<APIEventModel>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20%20%09fe.*%2C%20%20%20%20%20s.*%2C%20%20%20%20%20sl.*%20from%20featuredevent%20as%20fe%20join%20sponsor%20as%20s%20on%20s.sponsorid%3Dfe.sponsorid%20%20left%20join%20sponsorlevel%20as%20sl%20on%20sl.sponsorlevelid%3D%20s.sponsorlevelid")));
                return res;
            });

        }

        public static Task<List<MiniHack>> GetMiniHacks()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<MiniHack>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20%09mh.*%20from%20minihack%20as%20mh")));
                return res;
            });

        }

        public static Task<List<Speaker>> GetSpeakers()
		{
			return Task.Factory.StartNew(() =>
			{
				var res = Request.Get<List<Speaker>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20%09mh.*%20from%20speaker%20as%20mh")));
				return res;
			});

		}
        public static Task<List<Attendee>> GetAttendees()
		{
			return Task.Factory.StartNew(() =>
			{
				var res = Request.Get<List<Attendee>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20*%20from%20attendees")));
				return res;
			});

		}
        public static Task<List<POICategory>> GetPOICategories()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<POICategory>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20*%20from%20poicategory")));
                return res;
            });

        }
        public static Task<List<POISubCategory>> GetPOISubCategories()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<POISubCategory>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20*%20from%20poisubcategory")));
                return res;
            });
        }
        public static Task<List<POIItem>> GetPOIItems()
        {
            return Task.Factory.StartNew(() =>
            {
                var res = Request.Get<List<POIItem>>(HttpWebRequest.Create(string.Format(BaseAPI, "select%20*%20from%20poiitems")));
                return res;
            });
        }
    }
}


