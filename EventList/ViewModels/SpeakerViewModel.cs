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
    public class SpeakerViewModel:ViewModelBase
    {
		public ObservableRangeCollection<Speaker> Speakers { get; } = new ObservableRangeCollection<Speaker>();

		public SpeakerViewModel(INavigation navigation) : base(navigation)
        {
        }
		ICommand loadSpeakersCommand;
		public ICommand LoadSpeakersCommand =>
			loadSpeakersCommand ?? (loadSpeakersCommand = new Command(async () => await ExecuteLoadSpeakersCommand()));

		public async Task ExecuteLoadSpeakersCommand()
		{
			if (IsBusy)
				return;

			try
			{
				IsBusy = true;

#if DEBUG
				await Task.Delay(1000);
#endif
                var realmspeakers = App._Realm.All<Speaker>();
				if (realmspeakers.Count().Equals(0))
				{
					await LoadSpeakersFromAPI();

                    foreach (Speaker speaker in Speakers)
					{
						App._Realm.Write(() =>
						{
							App._Realm.Add(speaker);
						});
					}
				}
				else
				{
                    Speakers.ReplaceRange(realmspeakers);
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
            List<Speaker> speakers = await APIServices.GetSpeakers();

			if (speakers != null && speakers.Count > 0)
			{
				foreach (var speaker in speakers)
				{
                    Speakers.Add(speaker);
				}
			}
		}
    }
}
