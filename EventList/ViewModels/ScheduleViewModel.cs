using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList
{
    public class ScheduleViewModel:ViewModelBase
    {
        public ObservableRangeCollection<ScheduleItemViewModel> Schedules { get; set; }

		public ScheduleViewModel(INavigation navigation) : base(navigation)
        {
        }
		ICommand loadSchedulesCommand;
		public ICommand LoadSchedulesCommand => loadSchedulesCommand ?? (loadSchedulesCommand = new Command(async () => await ExecuteLoadSchedulesAsync()));

		async Task<bool> ExecuteLoadSchedulesAsync()
		{
			if (IsBusy)
				return false;

			try
			{
				IsBusy = true;
#if DEBUG
				await Task.Delay(1000);
#endif
				Schedules = new ObservableRangeCollection<ScheduleItemViewModel>();

				List<Session> Sessions = new List<Session>();

				var sessions = App._Realm.All<Session>();
				var speakers = App._Realm.All<Models.Speaker>();
				foreach (var session in sessions)
				{
					session.SessionSpeakers = new List<Speaker>();
					foreach (var speaker in speakers)
					{
						if (speaker.SessionId.Equals(session.Id))
						{
							session.SessionSpeakers.Add(speaker);
						}
					}
					Sessions.Add(session);
				}

				var featuredevents = App._Realm.All<FeaturedEvent>();

				if (sessions.Count() == 0 || featuredevents.Count() == 0)
					return false;


                foreach (var session in Sessions)
                {
                    bool isreminderset = await ReminderService.HasReminderAsync(session.Id);
                    if (isreminderset)
                    {
                        Schedules.Add(new ScheduleItemViewModel()
                        {
                            ScheduledSession = session
                        });
                    }
                }
                foreach (var featuredevent in featuredevents)
				{
					bool isreminderset = await ReminderService.HasReminderAsync("event_" + featuredevent.Id);
					if (isreminderset)
					{
						Schedules.Add(new ScheduleItemViewModel()
						{
                            ScheduledEvent = featuredevent
						});
					}
				}	

                RaisePropertyChanged("Schedules");

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
    }
}
