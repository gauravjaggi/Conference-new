using System;
using Newtonsoft.Json;
namespace EventList.Models
{
    public class Venue
    {
        [JsonProperty(PropertyName = "zoom")]
        public string Zoom { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public string Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public string Latitude { get; set; }

    }
}
