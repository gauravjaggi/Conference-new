using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Models;
using EventList.Pages.Chat;
using MvvmHelpers;
using Realms.Sync;
using Xamarin.Forms;

namespace EventList
{
    public class ChatRoomVieModel : ViewModelBase
    {
        public Realms.Sync.User ChatRealmAdmin { get; set; }

        public Models.GoogleUser GoogleLoggedInUser { get; set; }

        public ObservableRangeCollection<Message> Messages { get; } = new ObservableRangeCollection<Message>();

        public string OutGoingText { get; set; }

        public ChatRoomVieModel(INavigation navigation) : base(navigation)
        {
        }
        	
		ICommand loadmessagescommand;
        public ICommand LoadMessagesCommand => loadmessagescommand ?? (loadmessagescommand = new Command<ListView>(async (listview) => await ExecuteLoadMessagesCommand(listview)));

        async Task ExecuteLoadMessagesCommand(ListView listview)
		{
            try
            {
                var messages = App._ChatRealm.All<Message>().ToList();
                if (messages != null && messages.Count > 0)
                {
                    Messages.ReplaceRange(messages);
                    listview.ScrollTo(Messages[Messages.Count() - 1], ScrollToPosition.End, false);
                }
            }
            catch(Exception ex)
            {
                
            }
		}

		ICommand sendmessagecommand;
		public ICommand SendMessageCommand => sendmessagecommand ?? (sendmessagecommand = new Command(async () => await ExecuteSendMessageCommand()));

        async Task ExecuteSendMessageCommand()
        {
            try
            {
                
                if (!string.IsNullOrEmpty(OutGoingText))
                {
                    Message newmessage = new Message()
                    {
                        UserId = GoogleLoggedInUser.UserId,
                        FullName=string.Format("{0} {1}",GoogleLoggedInUser.GivenName, GoogleLoggedInUser.FamilyName),
                        Text = OutGoingText,
                        MessageTime = DateTime.Now
                    };
                    App._ChatRealm.Write(()=>  
                    {
                         App._ChatRealm.Add(newmessage);
                    });
                    OutGoingText = string.Empty;
                    RaisePropertyChanged("OutGoingText");
                }
            }
            catch(Exception ex)
            {
                
            }
        }
        public async Task ConnectToChatDb(string token)
        {
            var credentials = Credentials.Google(token);
            ChatRealmAdmin = await User.LoginAsync(credentials, Constants.AuthServerUri);
            var chatconfig = new SyncConfiguration(ChatRealmAdmin, Constants.ChatSyncServerUri)
            {
                ObjectClasses = new[] { typeof(Message) }
            };
            App._ChatRealm = Realms.Realm.GetInstance(chatconfig);
        }
    }
}
