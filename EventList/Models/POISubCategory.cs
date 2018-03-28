using System;
using Newtonsoft.Json;

namespace EventList.Models
{
    public class POISubCategory:Realms.RealmObject
    {
        [JsonProperty(PropertyName = "subcatid")]
        public int SubCategoryID { get; set; }

        [JsonProperty(PropertyName = "catid")]
        public int CategoryId { get; set; }

        [JsonProperty(PropertyName = "tablesectionid")]
        public string TableSection { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
