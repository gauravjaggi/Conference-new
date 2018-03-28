using System;
namespace EventList.Models
{
    public class GoogleUser:Realms.RealmObject
    {
        public string UserId { get; set; }

        public string GivenName { get; set; }

        public string FamilyName { get; set; }

        public string Email { get; set; }

        public BackgroundColor BackgroundColor { get; set; }
    }
}
