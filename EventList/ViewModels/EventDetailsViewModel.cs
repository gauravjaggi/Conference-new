using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using FormsToolkit;
using Xamarin.Forms;

namespace EventList
{
public class EventDetailsViewModel : ViewModelBase
{
	public FeaturedEvent Event { get; set; }

    Helpers.INotificationManager notificationService;

	public EventDetailsViewModel(INavigation navigation, FeaturedEvent e) : base(navigation)
	{
        notificationService = DependencyService.Get<Helpers.INotificationManager>();  
		Event = e;		
	}

	bool isReminderSet;
	public bool IsReminderSet
	{
		get { return isReminderSet; }
		set { SetProperty(ref isReminderSet, value); }
	}

	ICommand loadEventDetailsCommand;
	public ICommand LoadEventDetailsCommand =>
		loadEventDetailsCommand ?? (loadEventDetailsCommand = new Command(async () => await ExecuteLoadEventDetailsCommandAsync()));

	async Task ExecuteLoadEventDetailsCommandAsync()
	{

		if (IsBusy)
			return;

		try
		{


			IsBusy = true;
			IsReminderSet = await ReminderService.HasReminderAsync("event_" + Event.Id);
			Helpers.IDeviceService deviceservice = DependencyService.Get<Helpers.IDeviceService>();
			string devicename = deviceservice.GetDeviceName();

			var favorites = App._Realm.All<Favorite>();// check if this session has been added as favorite in realm
            if (favorites.Any(x => x.Id == Event.Id && x.DeviceUser == devicename))
				Event.IsFavorite = true;

			var feedbacks = App._Realm.All<Feedback>();// check if this session has been given feedback before

			List<Feedback> eventfeedbackslist = feedbacks.Where(x => x.EventType == "Event").ToList();
			if (eventfeedbackslist.Count() > 0)
			{
                string eventid = string.Format("{0}-{1}", devicename, Event.Id);
				if (eventfeedbackslist.Any(a => a.Username == eventid))
					Event.FeedbackLeft = true;
			}
		}
		catch (Exception ex)
		{
			//MessagingService.Current.SendMessage(MessageKeys.Error, ex);
		}
		finally
		{
			IsBusy = false;
		}
	}

	ICommand reminderCommand;
	public ICommand ReminderCommand =>
		reminderCommand ?? (reminderCommand = new Command(async () => await ExecuteReminderCommandAsync()));


	async Task ExecuteReminderCommandAsync()
	{
		if (!IsReminderSet)
		{
			
			var result = await ReminderService.AddReminderAsync("event_" + Event.Id,
				new Plugin.Calendars.Abstractions.CalendarEvent
				{
					Description = Event.Description,
					Location = Event.LocationName,
					AllDay = Event.IsAllDay,
					Name = Event.Title,
     //               Start = Event.StartTime.Value.UtcDateTime,
					//End = Event.EndTime.Value.UtcDateTime
                    Start=DateTime.Now.AddMinutes(11),
                    End=DateTime.Now.AddDays(10)
				});


			if (!result)
                return;
            
			notificationService.CreateEventNotication(Event);	
			
			IsReminderSet = true;
		}
		else
		{
			var result = await ReminderService.RemoveReminderAsync("event_" + Event.Id);
			if (!result)
				return;			
			IsReminderSet = false;
            notificationService.DeleteEventNotification(Event);
		}

	}
	ICommand favoriteCommand;
	public ICommand FavoriteCommand =>
		favoriteCommand ?? (favoriteCommand = new Command(async () => await ExecuteFavoriteCommandAsync()));

	async Task ExecuteFavoriteCommandAsync()
	{
		//await FavoriteService.ToggleFavorite(Session);
		Helpers.IDeviceService deviceservice = DependencyService.Get<Helpers.IDeviceService>();
		string devicename = deviceservice.GetDeviceName();
		Event.IsFavorite = !Event.IsFavorite;
		if (Event.IsFavorite)
		{
			//add to realm db
			App._Realm.Write(() =>
			{
				App._Realm.Add(new Favorite()
				{
					DeviceUser = devicename,
					Id = Event.Id,
					EventType = "Event"
				});
			});
		}
		else
		{
			//delete from db
            var favorite = App._Realm.All<Favorite>().First(b => b.DeviceUser == devicename && b.Id == Event.Id);//Select appropriate from realm favorite list

			// Delete an object with a transaction
			using (var trans = App._Realm.BeginWrite())
			{
				App._Realm.Remove(favorite);
				trans.Commit();
			}
		}

	}
	Sponsor selectedSponsor;
	public Sponsor SelectedSponsor
	{
		get { return selectedSponsor; }
		set
		{
			selectedSponsor = value;
			OnPropertyChanged();
			if (selectedSponsor == null)
				return;

			MessagingService.Current.SendMessage(MessageKeys.NavigateToSponsor, selectedSponsor);

			SelectedSponsor = null;
		}
	}

}
}