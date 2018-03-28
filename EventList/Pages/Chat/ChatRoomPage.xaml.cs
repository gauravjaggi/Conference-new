using System;
using System.Collections.Generic;
using System.Linq;
using EventList.Helpers;
using EventList.Models;
using Xamarin.Forms;

namespace EventList.Pages.Chat
{
    public partial class ChatRoomPage : ContentPage
    {
        ChatRoomVieModel vm;
        public ChatRoomPage(Models.GoogleUser googleuser)
        {
            InitializeComponent();
            BindingContext = vm = new ChatRoomVieModel(Navigation);
            NavigationPage.SetHasBackButton(this, false);
            vm.GoogleLoggedInUser = googleuser;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App._ChatRealm != null)
            {
                App._ChatRealm.RealmChanged -= MessageReceived;
                App._ChatRealm.RealmChanged += MessageReceived;
            }
            var messages = App._ChatRealm.All<Message>().ToList();
            if (messages != null && messages.Count > 0)
                vm.Messages.ReplaceRange(messages);
            //var lastChild = parentLayout.Children.LastOrDefault();
            //if (lastChild != null)
            //    scrollView.ScrollToAsync(lastChild, ScrollToPosition.End, true);
        }
        public void MessageReceived(object sender, EventArgs e)
        {
            var m = sender as Realms.Realm;

            vm.LoadMessagesCommand.Execute(lvMessages);
        }
        public void OnSendClicked(object sender, EventArgs e)
        {
            txtMessage.Unfocus();
            vm.SendMessageCommand.Execute(null);
        }
        public void OnLogout(object sender, EventArgs e)
        {
            if (Device.OS.Equals(TargetPlatform.Android))
            {
                var service = DependencyService.Get<IAndroidAuth>();
                service.context = this;
                service.Logout();
            }
            else
            {
                var dependencyservice = DependencyService.Get<IGoogleService>();
                dependencyservice.context = this;
                dependencyservice.Logout();               
            }
        }
		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			lvMessages.SelectedItem = null;
		}

		void OnItemTapped(object sender, ItemTappedEventArgs e)
		{
			lvMessages.SelectedItem = null;
		}
    }
}
