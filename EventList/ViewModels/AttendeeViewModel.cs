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
    public class AttendeeViewModel:ViewModelBase
    {
		public ObservableRangeCollection<Attendee> Attendees { get; } = new ObservableRangeCollection<Attendee>();

		public AttendeeViewModel(INavigation navigation) : base(navigation)
        {
		}
		ICommand loadAttendeesCommand;
		public ICommand LoadAttendeesCommand =>
			loadAttendeesCommand ?? (loadAttendeesCommand = new Command(async () => await ExecuteLoadAttendeesCommand()));

		public async Task ExecuteLoadAttendeesCommand()
		{
			if (IsBusy)
				return;

			try
			{
				IsBusy = true;

#if DEBUG
				await Task.Delay(1000);
#endif
                var realmattendees = App._Realm.All<Attendee>();
				if (realmattendees.Count().Equals(0))
				{
					await LoadSpeakersFromAPI();

                    foreach (Attendee attendee in Attendees)
					{
						App._Realm.Write(() =>
						{
							App._Realm.Add(attendee);
						});
					}
				}
				else
				{
					Attendees.ReplaceRange(realmattendees);
				}

			}
			catch (Exception ex)
			{

			}
			finally
			{
				IsBusy = false;
			}
		}
		public async Task LoadSpeakersFromAPI()
		{
            List<Attendee> attendees = await APIServices.GetAttendees();

			if (attendees != null && attendees.Count > 0)
			{
				foreach (var attendee in attendees)
				{
                    Attendees.Add(attendee);
				}
			}
		}
    }
}
