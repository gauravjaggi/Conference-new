using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using FormsToolkit;
using MvvmHelpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace EventList.ViewModels
{
	public class SessionsViewModel : ViewModelBase
	{
		public SessionsViewModel(INavigation navigation) : base(navigation)
		{
			NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
		}
        public static List<Models.SessionDataModel> SessionDataModel { get; set; }

		public ObservableRangeCollection<Session> Sessions { get; } = new ObservableRangeCollection<Session>();
		public ObservableRangeCollection<Session> SessionsFiltered { get; } = new ObservableRangeCollection<Session>();
		public ObservableRangeCollection<Grouping<string, Session>> SessionsGrouped { get; } = new ObservableRangeCollection<Grouping<string, Session>>();
		public DateTime NextForceRefresh { get; set; }


		#region Properties
		Session selectedSession;
		public Session SelectedSession
		{
			get { return selectedSession; }
			set
			{
				selectedSession = value;
				OnPropertyChanged();
				if (selectedSession == null)
					return;

				MessagingService.Current.SendMessage(MessageKeys.NavigateToSession, selectedSession);

				SelectedSession = null;
			}
		}

		string filter = string.Empty;
		public string Filter
		{
			get { return filter; }
			set
			{
				if (SetProperty(ref filter, value))
					ExecuteFilterSessionsAsync();

			}
		}
		#endregion

		#region Filtering and Sorting


		void SortSessions()
		{
			SessionsGrouped.ReplaceRange(SessionsFiltered.FilterAndGroupByDate());


		}


		bool noSessionsFound;
		public bool NoSessionsFound
		{
			get { return noSessionsFound; }
			set { SetProperty(ref noSessionsFound, value); }
		}

		string noSessionsFoundMessage;
		public string NoSessionsFoundMessage
		{
			get { return noSessionsFoundMessage; }
			set { SetProperty(ref noSessionsFoundMessage, value); }
		}

		#endregion


		#region Commands

		ICommand forceRefreshCommand;
		public ICommand ForceRefreshCommand =>
		forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

		async Task ExecuteForceRefreshCommandAsync()
		{
			await ExecuteLoadSessionsAsync(true);
		}

		ICommand filterSessionsCommand;
		public ICommand FilterSessionsCommand =>
			filterSessionsCommand ?? (filterSessionsCommand = new Command(async () => await ExecuteFilterSessionsAsync()));

		async Task ExecuteFilterSessionsAsync()
		{
			IsBusy = true;
			NoSessionsFound = false;

			// Abort the current command if the query has changed and is not empty
			if (!string.IsNullOrEmpty(Filter))
			{
				var query = Filter;
				await Task.Delay(250);
				if (query != Filter)
					return;
			}

			SessionsFiltered.ReplaceRange(Sessions.Search(Filter));
			SortSessions();

			if (SessionsGrouped.Count == 0)
			{
				if (Settings.Current.FavoritesOnly)
				{
					if (!Settings.Current.ShowPastSessions && !string.IsNullOrWhiteSpace(Filter))
						NoSessionsFoundMessage = "You haven't favorited\nany sessions yet.";
					else
						NoSessionsFoundMessage = "No Sessions Found";
				}
				else
					NoSessionsFoundMessage = "No Sessions Found";

				NoSessionsFound = true;
			}
			else
			{
				NoSessionsFound = false;
			}

			IsBusy = false;
		}


		ICommand loadSessionsCommand;
		public ICommand LoadSessionsCommand =>
			loadSessionsCommand ?? (loadSessionsCommand = new Command<bool>(async (f) => await ExecuteLoadSessionsAsync()));


		async Task<bool> ExecuteLoadSessionsAsync(bool force = false)
		{
            bool loadspeakers = false;
			if (IsBusy)
				return false;

			try
			{
				SessionDataModel = new List<Models.SessionDataModel>();
				NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
				IsBusy = true;
				NoSessionsFound = false;
				Filter = string.Empty;

#if DEBUG
				await Task.Delay(1000);
#endif
				var sessionsfromrealm = App._Realm.All<Models.Session>();

				if (sessionsfromrealm.Count().Equals(0))
				{
                    await LoadSessionsFromAPI();

                    var realmspeakers = App._Realm.All<Speaker>();

                    loadspeakers = realmspeakers.Count()==0;
                    foreach (Models.Session item in Sessions)
                    {
                        if (loadspeakers) { 
                        foreach (var speaker in item.SessionSpeakers)
                        {
                            App._Realm.Write(() =>
                            {
                                App._Realm.Add(speaker);
                            });
                        }
                    }
		              App._Realm.Write(() =>
		              {
		                  App._Realm.Add(item);
		                  App._Realm.Add(item.MainCategory);
		                  if(item.Room!=null)
		                      App._Realm.Add(item.Room);
		              });

		            }
				}
				else
				{
					var speakers = App._Realm.All<Models.Speaker>();

					foreach (var item in sessionsfromrealm)
					{
						item.SessionSpeakers = new List<Speaker>();
						foreach (var speaker in speakers)
						{
							if (speaker.SessionId.Equals(item.Id))
								item.SessionSpeakers.Add(speaker);
						}
						Sessions.Add(item);
					}

				}
				SessionsFiltered.ReplaceRange(Sessions);

				foreach (var item in Sessions)
				{
					//DateTime startdate = ((DateTime)item.StartTime.Value.UtcDateTime).ToLocalTime();
					//DateTime enddate = ((DateTime)item.EndTime.Value.UtcDateTime).ToLocalTime();
					SessionDataModel.Add(new SessionDataModel()
					{
						Id = item.Id,
						Title = item.Title,
						ShortTitle = item.ShortTitle,
						Abstract = item.Abstract,
						MainCategory = item.MainCategory,
						Room = item.Room,
						SessionSpeakers = item.SessionSpeakers,
						StartTime = DateTime.Now,//SpecifyKind(startdate, DateTimeKind.Utc),
						EndTime = DateTime.Now,///SpecifyKind(enddate, DateTimeKind.Utc),                       
						RemoteId = item.RemoteId
					});
				}
				string s = JsonConvert.SerializeObject(SessionDataModel);
				SortSessions();
				if (SessionsGrouped.Count == 0)
				{

					if (Settings.Current.FavoritesOnly)
					{
						if (!Settings.Current.ShowPastSessions)
							NoSessionsFoundMessage = "You haven't favorited\nany sessions yet.";
						else
							NoSessionsFoundMessage = "No Sessions Found";
					}
					else
						NoSessionsFoundMessage = "No Sessions Found";

					NoSessionsFound = true;
				}
				else
				{
					NoSessionsFound = false;
				}



			}
			catch (Exception ex)
			{
				MessagingService.Current.SendMessage(MessageKeys.Error, ex);
			}
			finally
			{
				IsBusy = false;
			}

			return true;
		}


		//		ICommand loadSessionsCommand;
		//		public ICommand LoadSessionsCommand =>
		//			loadSessionsCommand ?? (loadSessionsCommand = new Command<bool>(async (f) => await ExecuteLoadSessionsAsync()));


		//		async Task<bool> ExecuteLoadSessionsAsync(bool force = false)
		//		{
		//			if (IsBusy)
		//				return false;

		//			try
		//			{
		//                SessionDataModel = new List<Models.SessionDataModel>();
		//				NextForceRefresh = DateTime.UtcNow.AddMinutes(45);
		//				IsBusy = true;
		//				NoSessionsFound = false;
		//				Filter = string.Empty;

		//#if DEBUG
		//				await Task.Delay(1000);
		//#endif
		//              //Sessions.ReplaceRange(await StoreManager.SessionStore.GetItemsAsync(force));


		//		var sessionsfromrealm = App._Realm.All<Models.Session>();

		//	    if (sessionsfromrealm.Count().Equals(0))
		//	    {
		//         	IFileWriter fileEditor = DependencyService.Get<IFileWriter>();
		//			List<Session> sessions = fileEditor.GetSessionFromCsv();

		//          	foreach (Models.Session item in sessions)
		//          	{
		//				foreach (var speaker in item.SessionSpeakers)
		//				{ 
		//					App._Realm.Write(() =>
		//					{
		//						App._Realm.Add(speaker);
		//					});
		//				}
		//				App._Realm.Write(() =>
		//				{
		//					App._Realm.Add(item);
		//					App._Realm.Add(item.MainCategory);
		//					if(item.Room!=null)
		//						App._Realm.Add(item.Room);
		//				});

		//	        }
		//			Sessions.ReplaceRange(sessions);
		//      	}
		//     	else
		//      	{
		//			var speakers = App._Realm.All<Models.Speaker>();

		//			foreach (var item in sessionsfromrealm)
		//			{
		//				item.SessionSpeakers = new List<Speaker>();
		//				foreach (var speaker in speakers)
		//				{
		//					if (speaker.SessionId.Equals(item.Id))
		//						item.SessionSpeakers.Add(speaker);
		//				}
		//				Sessions.Add(item);
		//			}

		//      	}
		//              //string s = JsonConvert.SerializeObject(Sessions);
		//		SessionsFiltered.ReplaceRange(Sessions);

		//		foreach (var item in Sessions)
		//		{
		//			//DateTime startdate = ((DateTime)item.StartTime.Value.UtcDateTime).ToLocalTime();
		//			//DateTime enddate = ((DateTime)item.EndTime.Value.UtcDateTime).ToLocalTime();
		//                  SessionDataModel.Add(new SessionDataModel()
		//                  {
		//                      Id = item.Id,
		//                      Title = item.Title,
		//                      ShortTitle = item.ShortTitle,
		//                      Abstract = item.Abstract,
		//                      MainCategory = item.MainCategory,
		//                      Room = item.Room,
		//                      SessionSpeakers = item.SessionSpeakers,
		//				StartTime = DateTime.Now,//SpecifyKind(startdate, DateTimeKind.Utc),
		//				EndTime = DateTime.Now,///SpecifyKind(enddate, DateTimeKind.Utc),						
		//				RemoteId = item.RemoteId
		//			});
		//		}
		//		string s = JsonConvert.SerializeObject(SessionDataModel);
		//		SortSessions();
		//              LoadSessionsFromAPI();
		//			if (SessionsGrouped.Count == 0)
		//		{

		//			if (Settings.Current.FavoritesOnly)
		//			{
		//				if (!Settings.Current.ShowPastSessions)
		//					NoSessionsFoundMessage = "You haven't favorited\nany sessions yet.";
		//				else
		//					NoSessionsFoundMessage = "No Sessions Found";
		//			}
		//			else
		//				NoSessionsFoundMessage = "No Sessions Found";

		//			NoSessionsFound = true;
		//		}
		//		else
		//		{
		//			NoSessionsFound = false;
		//		}



		//	}
		//	catch (Exception ex)
		//	{
		//		MessagingService.Current.SendMessage(MessageKeys.Error, ex);
		//	}
		//	finally
		//	{
		//		IsBusy = false;
		//	}

		//	return true;
		//}

		ICommand favoriteCommand;
		public ICommand FavoriteCommand =>
			favoriteCommand ?? (favoriteCommand = new Command<Session>(async (s) => await ExecuteFavoriteCommandAsync(s)));

		async Task ExecuteFavoriteCommandAsync(Session session)
		{
			var toggled = await FavoriteService.ToggleFavorite(session);
			if (toggled && Settings.Current.FavoritesOnly)
				SortSessions();
		}

		public void WriteCSVFiles(ObservableRangeCollection<Models.Session> list)
		{
			StringBuilder csvData = null;
			StringBuilder speakercsv = null;
			StringBuilder roomcsv = null;
			StringBuilder catcsv = null;
			try
			{
				csvData = new StringBuilder();
				speakercsv= new StringBuilder();
				roomcsv = new StringBuilder();
				catcsv = new StringBuilder();

				//Get the properties for type T for the headers
				List<PropertyInfo> props = typeof(Session).GetRuntimeProperties().ToList();
				List<PropertyInfo> speakerprops = null;
				List<PropertyInfo> roomprops = null;
				List<PropertyInfo> catprops = null;
				for (int i = 0; i <= props.Count-1; i++)
				{
					string name = props[i].Name;
					if (name.Equals("Speakers") || name.Equals("Room") || name.Equals("MainCategory")) 
					{
						if (name.Equals("Speakers"))
						{ 
							speakerprops = typeof(Speaker).GetRuntimeProperties().ToList();
							for (int k = 0; k <= speakerprops.Count - 1; k++)
							{
								speakercsv.Append(speakerprops[k].Name);
								if (k < speakerprops.Count - 1)
								{
									speakercsv.Append(",");
								}
							}
							speakercsv.AppendLine();
						}
						if (name.Equals("Room"))
						{ 
							roomprops = typeof(Room).GetRuntimeProperties().ToList();
							for (int m = 0; m <= roomprops.Count - 1; m++)
							{
								roomcsv.Append(roomprops[m].Name);
								if (m < roomprops.Count - 1)
								{
									roomcsv.Append(",");
								}
							}
							roomcsv.AppendLine();
						}
						if (name.Equals("MainCategory"))
						{
							catprops = typeof(Category).GetRuntimeProperties().ToList();
							for (int n = 0; n <= catprops.Count - 1;n++)
							{
								catcsv.Append(catprops[n].Name);
								if (n < catprops.Count - 1)
								{
									catcsv.Append(",");
								}
							}
							catcsv.AppendLine();
						}
					}
					else
					{
						csvData.Append(props[i].Name);
						if (i < props.Count - 1)
						{
							csvData.Append(",");
						}
					}
				}
				csvData.AppendLine();

				//Loop through the collection, then the properties and add the values
				for (int i = 0; i <= list.Count-1; i++)
				{
					Session item = list[i];
					for (int j = 0; j <= props.Count- 1; j++)
					{
						string name = props[j].Name;
						if (name.Equals("Speakers") || name.Equals("Room") || name.Equals("MainCategory")) 
						{
							if (name.Equals("Speakers"))
							{
								object csvProperty = item.GetType().GetRuntimeProperty(name).GetValue(item, null);
								ICollection<Speaker> coll = (ICollection<Speaker>)csvProperty; 
								GetSpeakerCSV(speakercsv,coll.ToList());
							}
							if (name.Equals("Room"))
							{
								object roomProperty = item.GetType().GetRuntimeProperty(name).GetValue(item, null);
								Room roomobj = (Room)roomProperty;
								GetRoomCSV(roomcsv,roomobj);
							}
							if (name.Equals("MainCategory"))
							{
								object roomProperty = item.GetType().GetRuntimeProperty(name).GetValue(item, null);
								Category catobj = (Category)roomProperty;
								GetCatCSV(catcsv, catobj);
							}
						}
						else
						{
							object csvProperty = item.GetType().GetRuntimeProperty(props[j].Name).GetValue(item, null);
							if (csvProperty != null)
							{
								string value = csvProperty.ToString();
								//Check if the value contans a comma and place it in quotes if so
								if (value.Contains(","))
								{
									value = value.Replace(',', '&');// string.Concat("\"", value, "\"");
								}
								//Replace any \r or \n special characters from a new line with a space
								if (value.Contains("\r"))
								{
									value = value.Replace("\r", " ");
								}
								if (value.Contains("\n"))
								{
									value = value.Replace("\n", " ");
								}
								csvData.Append(value);
							}
							if (j < props.Count - 1)
							{
								csvData.Append(",");
							}
						}
					}
					csvData.AppendLine();
				}
				//Write Files Session, Speaker, Room, Category
				IFileWriter filewriter = DependencyService.Get<IFileWriter>();
				filewriter.CreateCsvFile(csvData.ToString(),"Session.csv");
				filewriter.CreateCsvFile(speakercsv.ToString(),"Speaker.csv");
				filewriter.CreateCsvFile(roomcsv.ToString(),"Room.csv");
				filewriter.CreateCsvFile(catcsv.ToString(),"Category.csv");
			}
			catch (Exception ex)
			{
				//Exception
			}
			//return csvData.ToString();
		}

		public string GetSpeakerCSV(StringBuilder sb,List<Speaker> list)
		{
			
			try
			{
				//Get the properties for type T for the headers
				List<PropertyInfo> props = typeof(Speaker).GetRuntimeProperties().ToList();

				//Loop through the collection, then the properties and add the values
				for (int i = 0; i <= list.Count - 1; i++)
				{
					Speaker item = list[i];
					for (int j = 0; j <= props.Count - 1; j++)
					{
						object csvProperty = item.GetType().GetRuntimeProperty(props[j].Name).GetValue(item, null);
						if (csvProperty != null)
						{
							string value = csvProperty.ToString();
							//Check if the value contans a comma and place it in quotes if so
							if (value.Contains(","))
							{
								value = string.Concat("\"", value, "\"");
							}
							//Replace any \r or \n special characters from a new line with a space
							if (value.Contains("\r"))
							{
								value = value.Replace("\r", " ");
							}
							if (value.Contains("\n"))
							{
								value = value.Replace("\n", " ");
							}
							sb.Append(value);
						}
						if (j < props.Count - 1)
						{
							sb.Append(",");
						}

					}
					sb.AppendLine();
				}
			}
			catch (Exception ex)
			{
				//Exception
			}
			return sb.ToString();
		}
		public string GetRoomCSV(StringBuilder sb, Room obj)
		{

			try
			{
				//Get the properties for type T for the headers
				List<PropertyInfo> props = typeof(Room).GetRuntimeProperties().ToList();

				//Loop through the collection, then the properties and add the values

					for (int j = 0; j <= props.Count - 1; j++)
					{
						object csvProperty = obj.GetType().GetRuntimeProperty(props[j].Name).GetValue(obj, null);
						if (csvProperty != null)
						{
							string value = csvProperty.ToString();
							//Check if the value contans a comma and place it in quotes if so
							if (value.Contains(","))
							{
								value = string.Concat("\"", value, "\"");
							}
							//Replace any \r or \n special characters from a new line with a space
							if (value.Contains("\r"))
							{
								value = value.Replace("\r", " ");
							}
							if (value.Contains("\n"))
							{
								value = value.Replace("\n", " ");
							}
							sb.Append(value);
						}
						if (j < props.Count - 1)
						{
							sb.Append(",");
						}

					}
					sb.AppendLine();

			}
			catch (Exception ex)
			{
				//Exception
			}
			return sb.ToString();
		}
		public string GetCatCSV(StringBuilder sb, Category obj)
		{

			try
			{
				//Get the properties for type T for the headers
				List<PropertyInfo> props = typeof(Category).GetRuntimeProperties().ToList();

				//Loop through the collection, then the properties and add the values

				for (int j = 0; j <= props.Count - 1; j++)
				{
					object csvProperty = obj.GetType().GetRuntimeProperty(props[j].Name).GetValue(obj, null);
					if (csvProperty != null)
					{
						string value = csvProperty.ToString();
						//Check if the value contans a comma and place it in quotes if so
						if (value.Contains(","))
						{
							value = string.Concat("\"", value, "\"");
						}
						//Replace any \r or \n special characters from a new line with a space
						if (value.Contains("\r"))
						{
							value = value.Replace("\r", " ");
						}
						if (value.Contains("\n"))
						{
							value = value.Replace("\n", " ");
						}
						sb.Append(value);
					}
					if (j < props.Count - 1)
					{
						sb.Append(",");
					}

				}
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				//Exception
			}
			return sb.ToString();
		}
		#endregion


        public async Task  LoadSessionsFromAPI()
        {
            Sessions.Clear();
			Session s = null;
            Category c = null;
            Room r = null;
			List<Speaker> speakers =null;
			List<APISessionModel> res = await APIServices.GetSession();
         
            if (res!=null && res.Count>0)
			{
                var list = res.GroupBy(x => x.SessionIid);
                foreach(var item in list)
                {
                    speakers = new List<Speaker>();
                    foreach (var subitem in item)
                    {
                        speakers.Add(new Speaker(){
                            SessionId= subitem.SessionIid,
                            Id=subitem.SpeakerId,
                            AvatarUrl=subitem.AvatarUrl,
                            Biography=subitem.Biography,
                            BlogUrl=subitem.BlogUrl,
                            CompanyName=subitem.CompanyName,
                            CompanyWebsiteUrl=subitem.CompanyWebsiteUrl,
                            Email=subitem.Email,
                            FirstName=subitem.FirstName,
                            LastName=subitem.LastName,
                            LinkedInUrl=subitem.LinkdinUrl,
                            PhotoUrl=subitem.PhotoUrl,
                            PositionName=subitem.PositionName,
                            TwitterUrl=subitem.TwitterUrl
                        });
                    }
                    var defaultitem = item.FirstOrDefault(); 
                    c = new Category() {
                        Id=defaultitem.CategoryId,
                        Name=defaultitem.CategoryName,
                        Color=defaultitem.Color,
                        ShortName=defaultitem.ShortName
                    };
                    r = new Room()
					{
                        Id = defaultitem.RoomId,
                        Name = defaultitem.RoomName,
                        ImageUrl=defaultitem.ImageUrl,
                        Latitude=defaultitem.Latitude,
                        Longitude=defaultitem.Longitude
					};
                    Sessions.Add(new Session(){
		                    Id = defaultitem.SessionIid,
		                    Title = defaultitem.Title,
		                    ShortTitle = defaultitem.ShortTitle,
		                    Abstract = defaultitem.Abstract,
		                    StartTime = defaultitem.StartTime,
		                    EndTime = defaultitem.EndTime,
		                    MainCategory = c,
		                    Room=r ,
                            SessionSpeakers=speakers
					});
			    }
			}
        }
	}
}

