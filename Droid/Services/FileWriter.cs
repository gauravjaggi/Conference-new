
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.App;
using Android.Content.Res;
using EventList.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(EventList.Droid.Services.FileWriter))]
namespace EventList.Droid.Services
{
    public class FileWriter:IFileWriter
    {
        Activity activity;
        AssetManager assets;
        string speakerstring = string.Empty;

        public FileWriter()
        {
            activity = Forms.Context as Activity;
            assets = activity.Assets;
        }

		
        public void CreateCsvFile(string csv, string filename)
        {
            //throw new NotImplementedException();
        }

		public List<Session> GetSessionFromCsv()
		{
			List<Models.Session> sessions = new List<Models.Session>();
			List<Models.Speaker> speakers = new List<Models.Speaker>();
			List<Models.Room> rooms = new List<Models.Room>();
			List<Models.Category> categories = new List<Models.Category>();

            List<string> speakerlines= ReadCSVFile("Speaker.csv");
            foreach (var line in speakerlines)
			{
				speakers.Add(GetSpeakers(line));
			}

            List<string> roomlines = ReadCSVFile("Room.csv");
			foreach (var line in roomlines)
			{
				rooms.Add(GetRooms(line));
			}

            List<string> catlines =ReadCSVFile("Category.csv");
			foreach (var line in catlines)
			{
				categories.Add(GetCategories(line));
			}

			List<string> sessionlines = ReadCSVFile("Session.csv");
			foreach (var line in sessionlines)
			{
				sessions.Add(GetSession(line, speakers, rooms, categories));
			}

			return sessions;
		}
		public Models.Session GetSession(string csvLine, List<Models.Speaker> speakers, List<Models.Room> rooms, List<Models.Category> categories)
		{
			string[] values = csvLine.Split(',');
			Models.Session session = new Models.Session()
			{
				Title = values[0],
				ShortTitle = values[1],
				Abstract = values[2],
				//StartTime = string.IsNullOrEmpty(values[3])?DateTime.Now:Convert.ToDateTime(values[3].Replace('/','-')),
				//EndTime = string.IsNullOrEmpty(values[4]) ? DateTime.Now : Convert.ToDateTime(values[4].Replace('/','-')),
				RemoteId = values[10],
				Id = values[11]
			};
			session.Room = rooms.Where(x => x.SessionId.Equals(session.Id)).FirstOrDefault();
			session.MainCategory = categories.Where(x => x.SessionId.Equals(session.Id)).FirstOrDefault();
			var lis = speakers.Where(x => x.SessionId.Equals(session.Id)).ToList();
			session.SessionSpeakers = lis;

			return session;
		}
		public Models.Speaker GetSpeakers(string csvLine)
		{
			string[] values = csvLine.Split(',');
			Models.Speaker speaker = new Models.Speaker()
			{
				SessionId = values[0],
				FirstName = values[1],
				LastName = values[2],
				Biography = values[3],
				PhotoUrl = values[4],
				AvatarUrl = values[5],
				PositionName = values[6],
				CompanyName = values[7],
				CompanyWebsiteUrl = values[8],
				BlogUrl = values[9],
				TwitterUrl = values[10],
				LinkedInUrl = values[11],
				Email = values[16],
				RemoteId = values[17],
				Id = values[18]
			};

			return speaker;
		}
		public Models.Room GetRooms(string csvLine)
		{
			string[] values = csvLine.Split(',');
			Models.Room room = new Models.Room()
			{
				SessionId = values[0],
				Name = values[1],
				ImageUrl = values[2],
				Latitude = string.IsNullOrEmpty(values[3]) ? 0 : Convert.ToDouble(values[3]),
				Longitude = string.IsNullOrEmpty(values[4]) ? 0 : Convert.ToDouble(values[4]),
				RemoteId = values[5],
				Id = values[6]
			};

			return room;
		}
		public Models.Category GetCategories(string csvLine)
		{
			string[] values = csvLine.Split(',');
			Models.Category category = new Models.Category()
			{
				SessionId = values[0],
				Name = values[1],
				ShortName = values[2],
				Color = values[3],
				RemoteId = values[4],
				Id = values[5]
			};

			return category;
		}
        public List<FeaturedEvent> GetEventsFromCsv()
        {
			List<FeaturedEvent> events = new List<FeaturedEvent>();
			List<Sponsor> sponsors = new List<Sponsor>();
			List<SponsorLevel> sponsorlevels = new List<SponsorLevel>();

            List<string> sponsorlevellines = ReadCSVFile("SponserLevel.csv");
			foreach (var line in sponsorlevellines)
			{
				sponsorlevels.Add(GetSponserLevel(line));
			}

			List<string> sponsorlines = ReadCSVFile("Sponser.csv");
			foreach (var line in sponsorlines)
			{
				sponsors.Add(GetSponser(line));
			}

			List<string> eventlines = ReadCSVFile("FeaturedEvent.csv");
			foreach (var line in eventlines)
			{
				events.Add(GetEvent(line, sponsorlevels, sponsors));
			}

			return events;
        }
		public SponsorLevel GetSponserLevel(string csvLine)
		{
			string[] values = csvLine.Split(',');
			SponsorLevel sponsorlevels = new SponsorLevel()
			{
				SponserId = values[0],
				Name = values[1],
				Rank = Convert.ToInt32(values[2]),
				RemoteId = values[3],
				Id = values[4]
			};

			return sponsorlevels;
		}
		public Sponsor GetSponser(string csvLine)
		{

			string[] values = csvLine.Split(',');

			Sponsor sponser = new Sponsor()
			{
				EventId = values[0],
				Name = values[1],
				Description = values[2],
				ImageUrl = values[3],
				WebsiteUrl = values[4],
				TwitterUrl = values[5],
				BoothLocation = string.IsNullOrEmpty(values[6]) ? string.Empty : values[6],
				Rank = int.Parse(values[7]),
				RemoteId = values[8],
				Id = values[9]
			};

			return sponser;
		}
		public FeaturedEvent GetEvent(string csvLine, List<SponsorLevel> levels, List<Sponsor> sponsors)
		{

			string[] values = csvLine.Split(',');

			FeaturedEvent featuredevent = new FeaturedEvent()
			{
				Title = values[0],
				Description = values[1],
				StartTime = DateTime.Now.AddMinutes(10),// string.IsNullOrEmpty(values[2]) ? DateTime.Now : Convert.ToDateTime(values[2]),
				EndTime = DateTime.Now.AddHours(8),// string.IsNullOrEmpty(values[3]) ? DateTime.Now : Convert.ToDateTime(values[3]),
				IsAllDay = values[4].Equals("FALSE") ? false : true,
				LocationName = values[5],
				RemoteId = values[8],
				Id = values[9]
			};
			Sponsor sponser = sponsors.Where(x => x.EventId.Equals(featuredevent.Id)).FirstOrDefault();
			SponsorLevel sponsorlevel = levels.Where(x => x.SponserId.Equals(sponser.Id)).FirstOrDefault();
			sponser.SponsorLevel = sponsorlevel;

			featuredevent.Sponsor = sponser;
			return featuredevent;
		}
        public List<MiniHack> GetMiniHacksFromCsv()
        {
			List<MiniHack> minihacks = new List<MiniHack>();

			List<string> minihacklines = ReadCSVFile("MiniHack.csv");
			foreach (var line in minihacklines)
			{
				minihacks.Add(GetMiniHack(line));
			}
			return minihacks;
        }
		public MiniHack GetMiniHack(string csvLine)
		{

			string[] values = csvLine.Split(',');

			MiniHack minihack = new MiniHack()
			{
				Name = values[0],
				Subtitle = values[1],
				Description = values[2],
				GitHubUrl = values[3],
				BadgeUrl = values[4],
				UnlockCode = values[5],
				IsCompleted = values[7].Equals("FALSE") ? false : true,
				RemoteId = values[8],
				Id = values[9]
			};

			return minihack;
		}
        public List<string> ReadCSVFile(string filename)
        {
            List<string> lines = new List<string>();
			using (StreamReader sr = new StreamReader(assets.Open(filename)))
			{
				string line;
				while ((line = sr.ReadLine()) != null)
				{
					lines.Add(line);
				}
			}
            return lines.Skip(1).ToList();
        }
    }
}
