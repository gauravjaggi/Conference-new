using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Xamarin.Forms;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Reflection;
using EventList.Helpers;
using EventList.Models;

namespace EventList
{
	public class EventsViewModel : ViewModelBase
	{
		public EventsViewModel(INavigation navigation) : base(navigation)
        {
			Title = "Events";
		}
        public static List<string> FeaturedEventsId { get; set; }

		public static List<Models.FeaturedEventDataModel> FeaturedEventDataModels { get; set; }

		public ObservableRangeCollection<FeaturedEvent> Events { get; } = new ObservableRangeCollection<FeaturedEvent>();
		public ObservableRangeCollection<Grouping<string, FeaturedEvent>> EventsGrouped { get; } = new ObservableRangeCollection<Grouping<string, FeaturedEvent>>();

		#region Properties
		FeaturedEvent selectedEvent;
		public FeaturedEvent SelectedEvent
		{
			get { return selectedEvent; }
			set
			{
				selectedEvent = value;
				OnPropertyChanged();
				if (selectedEvent == null)
					return;
		//MessagingService.Current.SendMessage(MessageKeys.NavigateToEvent, selectedEvent);
				SelectedEvent = null;
			}
		}

		public string SearchedText
		{
			get;
			set;
		}

		#endregion

		#region Sorting
		void SortEvents()
		{
			EventsGrouped.ReplaceRange(Events.GroupByDate());
		}

		#endregion

		#region Commands

		ICommand forceRefreshCommand;
		public ICommand ForceRefreshCommand => forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

		async Task ExecuteForceRefreshCommandAsync()
		{
			await ExecuteLoadEventsAsync(true);
		}
		ICommand loadEventsCommand;
		public ICommand LoadEventsCommand => loadEventsCommand ?? (loadEventsCommand = new Command<bool>(async (f) => await ExecuteLoadEventsAsync()));

		async Task<bool> ExecuteLoadEventsAsync(bool force = false)
		{
			if (IsBusy)
				return false;

			try
			{
				IsBusy = true;
#if DEBUG
				await Task.Delay(1000);
#endif

			    FeaturedEventDataModels = new List<Models.FeaturedEventDataModel>();
				var eventsfromrealm = App._Realm.All<FeaturedEvent>();
				if (eventsfromrealm.Count().Equals(0))
				{
                    await LoadEventsFromAPI();

                    foreach (FeaturedEvent item in Events)
					{
						App._Realm.Write(() =>
						{
							App._Realm.Add(item);
							App._Realm.Add(item.Sponsor);
							App._Realm.Add(item.Sponsor.SponsorLevel);
						});
					}
				}
				else
				{
					Events.ReplaceRange(eventsfromrealm);
				}
				foreach (var item in Events)
				{
					DateTime startdate = ((DateTime)item.StartTime.Value.UtcDateTime).ToLocalTime();
					DateTime enddate = ((DateTime)item.EndTime.Value.UtcDateTime).ToLocalTime();
					FeaturedEventDataModels.Add(new Models.FeaturedEventDataModel()
					{
						Id = item.Id,
						Title = item.Title,
						Description = item.Description,
						IsAllDay = item.IsAllDay,
						StartTime = DateTime.SpecifyKind(startdate, DateTimeKind.Utc),
						EndTime = DateTime.SpecifyKind(enddate, DateTimeKind.Utc),
						LocationName = item.LocationName,
						RemoteId = item.RemoteId,
						Sponsor = item.Sponsor
					});
				}
				SortEvents();
			}
			catch (Exception ex)
			{
				//MessagingService.Current.SendMessage(MessageKeys.Error, ex);
			}
			finally
			{
				IsBusy = false;
			}
			return true;
		}
		//ICommand loadEventsCommand;
		//public ICommand LoadEventsCommand => loadEventsCommand ?? (loadEventsCommand = new Command<bool>(async (f) => await ExecuteLoadEventsAsync()));

		//async Task<bool> ExecuteLoadEventsAsync(bool force = false)
		//{
		//	if (IsBusy)
		//		return false;
				
		//	try
		//	{
		//		IsBusy = true;
		//		#if DEBUG
  //              	await Task.Delay(1000);
		//		#endif

		//		//Events.ReplaceRange(await StoreManager.EventStore.GetItemsAsync(force));
		//		//Title = "Events (" + Events.Count(e => e.StartTime.HasValue && e.StartTime.Value > DateTime.UtcNow) + ")";
		//		//WriteCSVFiles(Events);

  //              FeaturedEventDataModels = new List<Models.FeaturedEventDataModel>();
		//		var eventsfromrealm = App._Realm.All<FeaturedEvent>();
		//		if (eventsfromrealm.Count().Equals(0))
		//		{
		//			IFileWriter fileEditor = DependencyService.Get<IFileWriter>();
		//			List<FeaturedEvent> featuredevents = fileEditor.GetEventsFromCsv();

		//			foreach (FeaturedEvent item in featuredevents)
		//			{
		//				App._Realm.Write(() =>
		//				{
		//					App._Realm.Add(item);
		//					App._Realm.Add(item.Sponsor);
		//					App._Realm.Add(item.Sponsor.SponsorLevel);
		//				});


		//			}
		//			Events.ReplaceRange(featuredevents);
                   
		//		}
		//		else
		//		{
  //                 Events.ReplaceRange(eventsfromrealm);
		//		}
		//		foreach (var item in Events)
		//		{
		//			DateTime startdate = ((DateTime)item.StartTime.Value.UtcDateTime).ToLocalTime();
		//			DateTime enddate = ((DateTime)item.EndTime.Value.UtcDateTime).ToLocalTime();
		//			FeaturedEventDataModels.Add(new Models.FeaturedEventDataModel()
		//			{
		//				Id = item.Id,
		//				Title = item.Title,
		//				Description = item.Description,
		//				IsAllDay = item.IsAllDay,
		//				StartTime = DateTime.SpecifyKind(startdate, DateTimeKind.Utc),
		//				EndTime = DateTime.SpecifyKind(enddate, DateTimeKind.Utc),
		//				LocationName = item.LocationName,
		//				RemoteId = item.RemoteId,
		//				Sponsor = item.Sponsor
		//			});
		//		}
		//		SortEvents();
		//	}
		//	catch (Exception ex)
		//	{
		//		//MessagingService.Current.SendMessage(MessageKeys.Error, ex);
		//	}
		//	finally
		//	{
		//		IsBusy = false;
		//	}
		//	return true;
		//}

		ICommand searchEventsCommand;
		public ICommand SearchEventsCommand => searchEventsCommand ?? (searchEventsCommand = new Command( () => ExecuteSearchEventsAsync()));

		async Task<bool> ExecuteSearchEventsAsync()
		{
			if (IsBusy)
				return false;
				
			try
			{
				IsBusy = true;
				#if DEBUG
                	await Task.Delay(1000);
#endif

				var items = await StoreManager.EventStore.GetItemsAsync(true);
				if (!string.IsNullOrEmpty(this.SearchedText))
				{
					Events.ReplaceRange(items.Where(c => c.Title.Contains(SearchedText)));
				}
				else
				{
					Events.ReplaceRange(items);
				}
				SortEvents();
			}
			catch (Exception ex)
			{
				//MessagingService.Current.SendMessage(MessageKeys.Error, ex);
			}
			finally
			{
				IsBusy = false;
			}
			return true;
		}

        #endregion

		public void WriteCSVFiles(ObservableRangeCollection<FeaturedEvent> list)
		{
			StringBuilder csvData = null;
			StringBuilder sponsercsv = null;
			try
			{
				csvData = new StringBuilder();
				sponsercsv = new StringBuilder();
				List<PropertyInfo> props = typeof(FeaturedEvent).GetRuntimeProperties().ToList();
				List<PropertyInfo> sponserprops = null;
				for (int i = 0; i <= props.Count - 1; i++)
				{
					string name = props[i].Name;
					if (name.Equals("Sponsor"))
					{	
						sponserprops = typeof(Sponsor).GetRuntimeProperties().ToList();
						for (int m = 0; m <= sponserprops.Count - 1; m++)
						{
							if (sponserprops[m].Name.Equals("SponsorLevel"))
							{ }
							else
							{
								sponsercsv.Append(sponserprops[m].Name);
								if (m < sponserprops.Count - 1)
								{
									sponsercsv.Append(",");
								}
							}
						}
						sponsercsv.AppendLine();
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
				for (int i = 0; i <= list.Count - 1; i++)
				{
					FeaturedEvent item = list[i];
					for (int j = 0; j <= props.Count - 1; j++)
					{
						string name = props[j].Name;
						if (name.Equals("Sponsor"))
						{

							object sponserProperty = item.GetType().GetRuntimeProperty(name).GetValue(item, null);
							Sponsor sponserobj = (Sponsor)sponserProperty;
							GetSponserCSV(sponsercsv, sponserobj);

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
				//Write Files FeaturedEvent
				IFileWriter filewriter = DependencyService.Get<IFileWriter>();
				filewriter.CreateCsvFile(csvData.ToString(), "FeaturedEvent.csv");
				filewriter.CreateCsvFile(sponsercsv.ToString(), "Sponser.csv");
			}
			catch (Exception ex)
			{
				//Exception
			}

		}
		public string GetSponserCSV(StringBuilder sb, Sponsor obj)
		{
			try
			{
				List<PropertyInfo> props = typeof(Sponsor).GetRuntimeProperties().ToList();
				for (int j = 0; j <= props.Count - 1; j++)
				{
					if (!props[j].Name.Equals("SponsorLevel"))
					{
						object csvProperty = obj.GetType().GetRuntimeProperty(props[j].Name).GetValue(obj, null);
						if (csvProperty != null)
						{
							string value = csvProperty.ToString();
							if (value.Contains(","))
							{
								//value = string.Concat("\"", value, "\"");
								value = value.Replace(',', '&');
							}
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

				}
				sb.AppendLine();

			}
			catch (Exception ex)
			{
				//Exception
			}
			return sb.ToString();
		}

		public void GetSponserLevelCsv(List<SponsorLevel> list)
		{
			StringBuilder csvData = null;
			try
			{
				csvData = new StringBuilder();
				List<PropertyInfo> props = typeof(SponsorLevel).GetRuntimeProperties().ToList();
				for (int i = 0; i <= props.Count - 1; i++)
				{
					csvData.Append(props[i].Name);
					if (i < props.Count - 1)
					{
						csvData.Append(",");
					}				
				}
				csvData.AppendLine();

				for (int i = 0; i <= list.Count - 1; i++)
				{
					SponsorLevel item = list[i];
					for (int j = 0; j <= props.Count - 1; j++)
					{
						string name = props[j].Name;

						object csvProperty = item.GetType().GetRuntimeProperty(props[j].Name).GetValue(item, null);
						if (csvProperty != null)
						{
							string value = csvProperty.ToString();
							if (value.Contains(","))
							{
								value = value.Replace(',', '&');
							}
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
					csvData.AppendLine();
				}
				//Write Files FeaturedEvent
				IFileWriter filewriter = DependencyService.Get<IFileWriter>();
				filewriter.CreateCsvFile(csvData.ToString(), "SponserLevel.csv");
			}
			catch (Exception ex)
			{
				//Exception
			}

		}
		public async Task LoadEventsFromAPI()
		{
		
            FeaturedEvent fe = null;
            Sponsor s = null;
		
			List<APIEventModel> res = await APIServices.GetEvents();

			if (res != null && res.Count > 0)
			{
				foreach (var item in res)
				{
                    SponsorLevel level = new SponsorLevel()
                    {   
                        Id= item.SponsorLevelId,
                        Name=item.Name,
                        Rank=item.Rank,
                    };
					s = new Sponsor()
					{
                        Id = item.SponsorId,
                        Name = item.SponsorName,
                        Rank = item.SponsorRank,
                        SponsorLevel=level,
                        TwitterUrl=item.TwitterUrl,
                        BoothLocation=item.BoothLocation,
                        Description=item.SponsorDescription,
                        EventId= item.EventId,
                        ImageUrl=item.ImageUrl,
                        WebsiteUrl=item.WebsiteUrl
					};

                    Events.Add(new FeaturedEvent()
					{
                        Id = item.EventId,
						Title = item.Title,
                        Description=item.Description,
                        Sponsor=s,
                        LocationName=item.LocationName,
                        IsAllDay=item.IsAllDay,
                        StartTime=item.StartTime,
                        EndTime=item.EndTime,
					});
				}
			}
		}
    }
}

