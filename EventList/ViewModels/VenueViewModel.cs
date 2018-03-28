using System;

using Xamarin.Forms;
using System.Windows.Input;
using System.Threading.Tasks;
using Plugin.ExternalMaps;
using Plugin.Messaging;
using FormsToolkit;

namespace EventList
{
    public class VenueViewModel:ViewModelBase
    {
		public bool CanMakePhoneCall => CrossMessaging.Current.PhoneDialer.CanMakePhoneCall;
		public string EventTitle => "Xamarin Evolve";
		public string LocationTitle => "Hyatt Regency Orlando";
		public string Address1 => "9801 International Drive";
		public string Address2 => "Orlando, FL 32819";
        public double Latitude { get; set; }
        public double Longitude { get; set; }

		ICommand navigateCommand;
		public ICommand NavigateCommand =>
			navigateCommand ?? (navigateCommand = new Command(async () => await ExecuteNavigateCommandAsync()));

		async Task ExecuteNavigateCommandAsync()
		{
			if (!await CrossExternalMaps.Current.NavigateTo(LocationTitle, Latitude, Longitude))
			{
				MessagingService.Current.SendMessage(MessageKeys.Message, new MessagingServiceAlert
				{
					Title = "Unable to Navigate",
					Message = "Please ensure that you have a map application installed.",
					Cancel = "OK"
				});
			}
		}

		ICommand callCommand;
		public ICommand CallCommand =>
			callCommand ?? (callCommand = new Command(ExecuteCallCommand));

		void ExecuteCallCommand()
		{
			var phoneCallTask = CrossMessaging.Current.PhoneDialer;
			if (phoneCallTask.CanMakePhoneCall)
				phoneCallTask.MakePhoneCall("14072841234");
		}
    }
}
