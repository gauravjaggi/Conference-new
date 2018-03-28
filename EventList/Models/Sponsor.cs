using System;
namespace EventList
{
    public class Sponsor : Realms.RealmObject
    {
        public string Id { get; set; }

        public string RemoteId { get; set; }

        public string EventId { get; set; }

        public string Name { get; set; }

        public SponsorLevel SponsorLevel { get; set; }

        public string Description { get; set; }

        public string ImageUrl { get; set; }

        public string WebsiteUrl { get; set; }

        public string TwitterUrl { get; set; }

        public string BoothLocation { get; set; }

        public int Rank { get; set; }

        //[Newtonsoft.Json.JsonIgnore]
        [Realms.Ignored]
        public Uri ImageUri
        {
            get
            {
                try
                {
                    return new Uri(ImageUrl);
                }
                catch
                {

                }
                return null;
            }
        }

    }
}