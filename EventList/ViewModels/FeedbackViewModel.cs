using System;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using FormsToolkit;
using Xamarin.Forms;

namespace EventList.ViewModels
{
    public class FeedbackViewModel : ViewModelBase
    {
        Session session;
        public Session Session
        {
            get { return session; }
            set { SetProperty(ref session, value); }
        }
        FeaturedEvent currentEvent;
		public FeaturedEvent CurrentEvent
		{
			get { return currentEvent; }
			set { SetProperty(ref currentEvent, value); }
		}
		public bool IsEvent { get; set; }
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
        public FeedbackViewModel(INavigation navigation, Session session, FeaturedEvent featuredevent, bool isevent) : base(navigation)
        {
            IsEvent = isevent;
            CurrentEvent = featuredevent;
            Session = session;
        }
        public FeedbackViewModel()
        {

        }
        ICommand submitRatingCommand;
        public ICommand SubmitRatingCommand =>
            submitRatingCommand ?? (submitRatingCommand = new Command(async () => await ExecuteSubmitRatingCommandAsync()));

        async Task ExecuteSubmitRatingCommandAsync()
        {
            try
			{
				Helpers.IDeviceService deviceservice = DependencyService.Get<Helpers.IDeviceService>();
				string devicename = deviceservice.GetDeviceName();
                
				App._Realm.Write(() =>{
                    App._Realm.Add(new Feedback(){
                        EventID=IsEvent?CurrentEvent.Id:Session.Id,
                        EventType=IsEvent?"Event":"Session",
                        ContentRating=ContentRating,
                        PresentationRating= PresentationRating,
                        WorkShopRating=WorkShopRating,
                        IsContentRelevant=IsContentRelevenant,
                        Comments=Comments,
                        Notes=Notes,
                        IsGoodToRecommend=IsRecommended,
                        PublicSpeakingRating=SkillRating,
                        Username=string.Format("{0}-{1}",devicename,IsEvent?CurrentEvent.Id : Session.Id)
                    });
				});
                if (IsEvent)
                    CurrentEvent.FeedbackLeft = true;
                else
                    Session.FeedbackLeft = true;
               await  Navigation.PopAsync(true);
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

