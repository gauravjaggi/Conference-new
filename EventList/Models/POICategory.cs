using System;
using Newtonsoft.Json;
namespace EventList.Models
{
    public class POICategory:Realms.RealmObject
    {
        [JsonProperty(PropertyName = "tabid")]
        public string Tab { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "catid")]
        public int CategoryID{ get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "image")]
        public string Image { get; set; }

        [JsonProperty(PropertyName = "tablesectionid")]
        public string TableSection { get; set; }

    }
}
