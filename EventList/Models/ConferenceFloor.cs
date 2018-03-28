using System;
using Newtonsoft.Json;

namespace EventList.Models
{
    public class ConferenceFloor
    {
        [JsonProperty(PropertyName = "image1")]
        public string Image1 { get; set; }

        [JsonProperty(PropertyName = "image2")]
        public string Image2 { get; set; }

    }
}
