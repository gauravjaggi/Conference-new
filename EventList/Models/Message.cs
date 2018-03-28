using System;
using System.Linq;
using Xamarin.Forms;

namespace EventList.Models
{
    public class Message:Realms.RealmObject
    {
        public string UserId { get; set; }

        public string FullName { get; set; }

        public string Text { get; set; }

        public DateTimeOffset MessageTime { get; set; }

        [Realms.Ignored]
        public Color Background{
            get{
                BackgroundColor c= App._Realm.All<GoogleUser>().Where(x => x.UserId.Equals(UserId)).FirstOrDefault().BackgroundColor;
                return Color.FromRgb(c.R, c.G, c.B);
            }
        } 
        [Realms.Ignored]
        public Color TextColor{
            get
            {
                return (((Background.R + Background.G + Background.B) / 3) > 128) ? Color.Black : Color.White;
            }
            
        } 
    }
}
