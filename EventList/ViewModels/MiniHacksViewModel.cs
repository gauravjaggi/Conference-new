using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using MvvmHelpers;
using Xamarin.Forms;
using System.Linq;
using FormsToolkit;
using System.Text;
using System.Collections.Generic;
using System.Reflection;

namespace EventList.ViewModels
{
	public class MiniHacksViewModel : ViewModelBase
	{
		public MiniHacksViewModel()
		{
		}

		public ObservableRangeCollection<MiniHack> MiniHacks { get; } = new ObservableRangeCollection<MiniHack>();

		bool noHacksFound;
		public bool NoHacksFound
		{
			get { return noHacksFound; }
			set { SetProperty(ref noHacksFound, value); }
		}

		#region Commands


		ICommand forceRefreshCommand;
		public ICommand ForceRefreshCommand =>
			forceRefreshCommand ?? (forceRefreshCommand = new Command(async () => await ExecuteForceRefreshCommandAsync()));

		async Task ExecuteForceRefreshCommandAsync()
		{
			await ExecuteLoadMiniHacksAsync(true);
		}


		ICommand loadMiniHacksCommand;
        public ICommand LoadMiniHacksCommand =>
		loadMiniHacksCommand ?? (loadMiniHacksCommand = new Command<bool>(async (f) => await ExecuteLoadMiniHacksAsync()));

		async Task<bool> ExecuteLoadMiniHacksAsync(bool force = false)
		{
			if (IsBusy)
				return false;

			try
			{
				IsBusy = true;
				NoHacksFound = false;

#if DEBUG
				await Task.Delay(1000);
#endif
				var minihacksfromrealm = App._Realm.All<Models.MiniHack>();
				if (minihacksfromrealm.Count().Equals(0))
				{
                    await LoadMiniHacksFromAPI();

					foreach (MiniHack item in MiniHacks)
					{
						App._Realm.Write(() =>
						{
							App._Realm.Add(item);
						});

					}
					foreach (var hack in MiniHacks)
						hack.IsCompleted = Settings.Current.IsHackFinished(hack.Id);
				}
				else
				{
					foreach (var hack in minihacksfromrealm)
						hack.IsCompleted = Settings.Current.IsHackFinished(hack.Id);
					MiniHacks.ReplaceRange(minihacksfromrealm);

				}
				NoHacksFound = MiniHacks.Count == 0;

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
		public async Task LoadMiniHacksFromAPI()
		{			
            List<MiniHack> res = await APIServices.GetMiniHacks();

			if (res != null && res.Count > 0)
			{
				foreach (var item in res)
				{
                    MiniHacks.Add(item);
				}
			}
		}
		//		ICommand loadMiniHacksCommand;
		//		public ICommand LoadMiniHacksCommand =>
		//		loadMiniHacksCommand ?? (loadMiniHacksCommand = new Command<bool>(async (f) => await ExecuteLoadMiniHacksAsync()));

		//		async Task<bool> ExecuteLoadMiniHacksAsync(bool force = false)
		//		{
		//			if (IsBusy)
		//				return false;

		//			try
		//			{
		//				IsBusy = true;
		//				NoHacksFound = false;

		//#if DEBUG
		//				await Task.Delay(1000);
		

		//		//var hacks = await StoreManager.MiniHacksStore.GetItemsAsync(force);
		//		//var finalHacks = hacks.ToList();

		//		//foreach (var hack in finalHacks)
		//		//	hack.IsCompleted = Settings.Current.IsHackFinished(hack.Id);


		//		//MiniHacks.ReplaceRange(finalHacks);

		//		var minihacksfromrealm = App._Realm.All<Models.MiniHack>();
		//		if (minihacksfromrealm.Count().Equals(0))
		//		{
		//			IFileWriter fileEditor = DependencyService.Get<IFileWriter>();
		//			List<MiniHack> minihacks = fileEditor.GetMiniHacksFromCsv();

		//			foreach (MiniHack item in minihacks)
		//			{
		//				App._Realm.Write(() =>
		//				{
		//					App._Realm.Add(item);
		//				});

		//			}
		//			foreach (var hack in minihacks)
		//				hack.IsCompleted = Settings.Current.IsHackFinished(hack.Id);
		//			MiniHacks.ReplaceRange(minihacks);
		//		}
		//		else
		//		{
		//			foreach (var hack in minihacksfromrealm)
		//				hack.IsCompleted = Settings.Current.IsHackFinished(hack.Id);
		//			MiniHacks.ReplaceRange(minihacksfromrealm);

		//		}
		//		NoHacksFound = MiniHacks.Count == 0;

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


		#endregion
		public void WriteCSVFile(ObservableRangeCollection<MiniHack> list)
		{
			StringBuilder csvData = null;
			try
			{
				csvData = new StringBuilder();
				List<PropertyInfo> props = typeof(MiniHack).GetRuntimeProperties().ToList();
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
					MiniHack item = list[i];
					for (int j = 0; j <= props.Count - 1; j++)
					{
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

				IFileWriter filewriter = DependencyService.Get<IFileWriter>();
				filewriter.CreateCsvFile(csvData.ToString(), "MiniHack.csv");
			}
			catch (Exception ex)
			{
				//Exception
			}
		}

	}
}

