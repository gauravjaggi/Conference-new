using System;
using Newtonsoft.Json;

namespace EventList.Models
{
    public class POIItem:Realms.RealmObject
    {
        [JsonProperty(PropertyName = "itemid")]
        public int ItemID { get; set; }

        [JsonProperty(PropertyName = "catid")]
        public int CategoryId { get; set; }

        [JsonProperty(PropertyName = "subcatid")]
        public int SubCategoryId { get; set; }

        [JsonProperty(PropertyName = "tablesectionid")]
        public string TableSection { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        [JsonProperty(PropertyName = "starttime")]
        public string StartTime { get; set; }

        [JsonProperty(PropertyName = "endtime")]
        public string EndTime { get; set; }

        [Realms.Ignored]
        public string Time { get{
                return string.Format("{0} {1} {2}", StartTime, string.IsNullOrEmpty(EndTime) ? "" : "-", EndTime);
           }}
    }
}
