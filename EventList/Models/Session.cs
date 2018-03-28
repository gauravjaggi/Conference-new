using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
namespace EventList.Models
{
    public class Session : Realms.RealmObject
    {
        public Session()
        {
            this.Speakers = new List<Speaker>();
        }

        public string Id { get; set; }

        public string RemoteId { get; set; }
       
        public string Title { get; set; }
              
        public string ShortTitle { get; set; }

        public string Abstract { get; set; }
           
        public IList<Speaker> Speakers { get; }

        public Room Room { get; set; }

        public Category MainCategory { get; set; }
               
        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? EndTime { get; set; }

        private string speakerNames;
        [Realms.Ignored]
        public string SpeakerNames
        {
            get
            {
                if (speakerNames != null)
                    return speakerNames;

                speakerNames = string.Empty;

                if (SessionSpeakers == null || SessionSpeakers.Count == 0)
                    return speakerNames;

                var allSpeakers = SessionSpeakers.ToArray();
                speakerNames = string.Empty;
                for (int i = 0; i < allSpeakers.Length; i++)
                {
                    speakerNames += allSpeakers[i].FullName;
                    if (i != SessionSpeakers.Count - 1)
                        speakerNames += ", ";
                }


                return speakerNames;
            }
        }

        [Realms.Ignored]
        public DateTime StartTimeOrderBy { get { return StartTime.HasValue ? StartTime.Value.UtcDateTime : DateTime.MinValue; } }
        const string delimiter = "|";
        string haystack;
        [Realms.Ignored]
        public string Haystack
        {
            get
            {
                if (haystack != null)
                    return haystack;

                var builder = new StringBuilder();
                builder.Append(delimiter);
                builder.Append(Title);
                builder.Append(delimiter);
                if (!string.IsNullOrWhiteSpace(MainCategory?.Name))
                    builder.Append(MainCategory.Name);
                builder.Append(delimiter);
                if (SessionSpeakers != null)
                {
                    foreach (var p in SessionSpeakers)
                        builder.Append($"{p.FirstName} {p.LastName}{delimiter}{p.FirstName}{delimiter}{p.LastName}");
                }

                haystack = builder.ToString();
                return haystack;
            }
        }

        bool isFavorite;
        [Realms.Ignored]
        public bool IsFavorite
        {
            get { return isFavorite; }
            set
            {
                isFavorite = value;
                RaisePropertyChanged("IsFavorite");
                //SetProperty(ref isFavorite, value);
            }
        }

        bool feedbackLeft;
        [Realms.Ignored]
        public bool FeedbackLeft
        {
            get { return feedbackLeft; }
            set
            {
                feedbackLeft = value;
                RaisePropertyChanged("FeedbackLeft");
                //SetProperty(ref feedbackLeft, value);
            }
        }
        [Realms.Ignored]
        public List<Speaker> SessionSpeakers { get; set; }
    }
}