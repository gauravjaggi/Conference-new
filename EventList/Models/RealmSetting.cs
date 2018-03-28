using System;
using Newtonsoft.Json;

namespace EventList.Models
{
    public class RealmSetting
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "syncserveruri")]
        public string SyncServerUri { get; set; }

        [JsonProperty(PropertyName = "chatsyncserveruri")]
        public string ChatSyncServerUri { get; set; }

        [JsonProperty(PropertyName = "synchost")]
        public string SyncHost { get; set; }

        [JsonProperty(PropertyName = "authserveruri")]
        public string AuthServerUri { get; set; }

        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }


}
