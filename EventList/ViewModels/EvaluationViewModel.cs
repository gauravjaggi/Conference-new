using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using Xamarin.Forms;

namespace EventList
{
    public class EvaluationViewModel:ViewModelBase
    {
		public int ContentRating { get; set; }
		public int PresentationRating { get; set; }
		public int SkillRating { get; set; }
		public int WorkShopRating { get; set; }
		public string Comments { get; set; }
		public string Notes { get; set; }


		public bool IsContentRelevenant { get; set; }
		public ImageSource ContentRelevanceIS
		{
			get
			{
				return IsContentRelevenant ? ImageSource.FromResource("EventList.Images.checkBox.png") : ImageSource.FromResource("EventList.Images.uncheckbox.png");
			}
		}
		public bool IsRecommended { get; set; }
		public ImageSource RecommendedIS
		{
			get
			{
				return IsRecommended ? ImageSource.FromResource("EventList.Images.checkBox.png") : ImageSource.FromResource("EventList.Images.uncheckbox.png");
			}
		}
        public EvaluationViewModel(INavigation navigation):base(navigation)
        {
        }

		ICommand evaluateCommand;
		public ICommand EvaluateCommand =>
			evaluateCommand ?? (evaluateCommand = new Command(async () => await ExecuteEvaluateCommandAsync()));

		async Task ExecuteEvaluateCommandAsync()
		{
			try
			{
				Helpers.IDeviceService deviceservice = DependencyService.Get<Helpers.IDeviceService>();
				string devicename = deviceservice.GetDeviceName();

				App._Realm.Write(() => {
					App._Realm.Add(new Feedback()
					{
						EventID = devicename,
						EventType = "Info",
						ContentRating = ContentRating,
						PresentationRating = PresentationRating,
						WorkShopRating = WorkShopRating,
						IsContentRelevant = IsContentRelevenant,
						Comments = Comments,
						Notes = Notes,
						IsGoodToRecommend = IsRecommended,
						PublicSpeakingRating = SkillRating,
                        Username = devicename
					});
				});
				await Navigation.PopAsync(true);
			}
			catch (Exception ex)
			{

			}
			finally
			{

			}
		}
    }
}
