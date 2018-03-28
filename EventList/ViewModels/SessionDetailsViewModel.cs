using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using FormsToolkit;
using Plugin.Share;
using Xamarin.Forms;

namespace EventList.ViewModels
{
	public class SessionDetailsViewModel : ViewModelBase
	{
		Helpers.INotificationManager notificationService;
		Session session;
		public Session Session
		{
			get { return session; }
			set { SetProperty(ref session, value); }
		}


		public SessionDetailsViewModel(INavigation navigation, Session session) : base(navigation)
		{
            notificationService = DependencyService.Get<Helpers.INotificationManager>();
			Session = session;
			if (Session.StartTime.HasValue)
				ShowReminder = !Session.StartTime.Value.UtcDateTime.IsTBA();
			else
				ShowReminder = false;
		}


		public bool ShowReminder { get; set; }

		bool isReminderSet;
		public bool IsReminderSet
		{
			get { return isReminderSet; }
			set { SetProperty(ref isReminderSet, value); }
		}



		Speaker selectedSpeaker;
		public Speaker SelectedSpeaker
		{
			get { return selectedSpeaker; }
			set
			{
				selectedSpeaker = value;
				OnPropertyChanged();
				if (selectedSpeaker == null)
					return;

				MessagingService.Current.SendMessage(MessageKeys.NavigateToSpeaker, selectedSpeaker);

				SelectedSpeaker = null;
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
            session.IsFavorite = !session.IsFavorite;
            if(session.IsFavorite)
            {
				//add to realm db
                App._Realm.Write(() =>
				{
                   App._Realm.Add(new Favorite(){
                        DeviceUser=devicename,
                        Id=session.Id,
                        EventType="Session"
                    });
				});
            }
            else
            {
				//delete from db
                var favorite = App._Realm.All<Favorite>().First(b => b.DeviceUser == devicename && b.Id==Session.Id);//Select appropriate from realm favorite list

				// Delete an object with a transaction
				using (var trans = App._Realm.BeginWrite())
				{
					App._Realm.Remove(favorite);
					trans.Commit();
				}
			}

		}

		ICommand reminderCommand;
		public ICommand ReminderCommand =>
			reminderCommand ?? (reminderCommand = new Command(async () => await ExecuteReminderCommandAsync()));

		async Task ExecuteReminderCommandAsync()
		{
			if (!IsReminderSet)
			{
				var result = await ReminderService.AddReminderAsync(Session.Id,
					new Plugin.Calendars.Abstractions.CalendarEvent
					{
						AllDay = false,
						Description = Session.Abstract,
						Location = Session.Room?.Name ?? string.Empty,
						Name = Session.Title,
						Start = Session.StartTime.Value.UtcDateTime,
						End = Session.EndTime.Value.UtcDateTime
					});


				if (!result)
					return;
                notificationService.CreateSessionNotication(Session);
				IsReminderSet = true;
			}
			else
			{
				var result = await ReminderService.RemoveReminderAsync(Session.Id);
				if (!result)
					return;				
				IsReminderSet = false;
			}

		}

		ICommand shareCommand;
		public ICommand ShareCommand =>
			shareCommand ?? (shareCommand = new Command(async () => await ExecuteShareCommandAsync()));

		async Task ExecuteShareCommandAsync()
		{
            Plugin.Share.Abstractions.ShareMessage message = new Plugin.Share.Abstractions.ShareMessage();
            message.Text = $"Can't wait for {Session.Title} at #XamarinEvolve!";
            message.Title = "Share";
			//await CrossShare.Current.Share($"Can't wait for {Session.Title} at #XamarinEvolve!", "Share");
            await CrossShare.Current.Share(message);
		}

		ICommand loadSessionCommand;
		public ICommand LoadSessionCommand =>
			loadSessionCommand ?? (loadSessionCommand = new Command(async () => await ExecuteLoadSessionCommandAsync()));

		public async Task ExecuteLoadSessionCommandAsync()
		{

			if (IsBusy)
				return;

			try
			{
                IsBusy = true;

				IsReminderSet = await ReminderService.HasReminderAsync(Session.Id);//check if reminder has been set for this session

				Helpers.IDeviceService deviceservice = DependencyService.Get<Helpers.IDeviceService>();
				string devicename = deviceservice.GetDeviceName();

                var favorites = App._Realm.All<Favorite>();// check if this session has been added as favorite in realm
                if (favorites.Any(x=>x.Id==Session.Id && x.DeviceUser==devicename))
                    Session.IsFavorite = true;


                var feedbacks = App._Realm.All<Feedback>();// check if this session has been given feedback before

                List<Feedback> sessionfeedbackslist = feedbacks.Where(x=>x.EventType=="Session").ToList();
                if(sessionfeedbackslist.Count()>0)
                {
                    string eventid = string.Format("{0}-{1}",devicename, Session.Id);
                    if (sessionfeedbackslist.Any(a => a.Username ==eventid ))
						Session.FeedbackLeft = true;
                }
				
				//Session.FeedbackLeft = await StoreManager.FeedbackStore.LeftFeedback(Session);


			}
			catch (Exception ex)
			{			
				MessagingService.Current.SendMessage(MessageKeys.Error, ex);
			}
			finally
			{
				IsBusy = false;
			}

		}



	}
}

