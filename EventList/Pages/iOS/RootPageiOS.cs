using System;
using EventList.Controls;
using EventList;
using EventList.Models;
using Xamarin.Forms;
using EventList.Pages.Home;
using EventList.Pages.CommonEvents;
using EventList.Pages.Sessions;
using EventList.Pages.MiniHacks;
using EventList.Pages.Events;
using EventList.Pages.Chat;
using EventList.Pages.Info;
using EventList.ViewModels;
using System.Threading.Tasks;

namespace EventList.Pages.iOS
{
	public class RootPageiOS : TabbedPage
	{
        FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);
        FeedViewModel vm;
		public RootPageiOS()
		{
            this.BackgroundImage = "TitleBackgoundImage.jpg";
           // BindingContext = new FeedViewModel(this.Navigation);
            NavigationPage.SetHasNavigationBar(this, false);
            Children.Add(new AppNavigationPage(new FeedsPage())
            {
                Icon = "home.png",
                Title = "Home",
           });
            Children.Add(new AppNavigationPage(new EventPage())
            {
				Icon = "home2.png",
				Title = "Events",
			});
            Children.Add(new AppNavigationPage(new Accounts.LoginPage())
			{
				Icon = "home3.png",
				Title = "Chat",
			});
			Children.Add(new AppNavigationPage(new InfoPage())
			{
				Icon = "home4.png",
				Title = "Info",
			});
		}		
        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();

        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await ConnectToRealmDb();

        }
		public void NavigateAsync(AppPage menuId)
		{
			switch ((int)menuId)
			{
				case (int)AppPage.Feed: CurrentPage = Children[0]; break;
				case (int)AppPage.Sessions: CurrentPage = Children[1]; break;
				case (int)AppPage.Events: CurrentPage = Children[2]; break;
				case (int)AppPage.MiniHacks: CurrentPage = Children[3]; break;
				case (int)AppPage.Information: CurrentPage = Children[4]; break;
				case (int)AppPage.Notification: CurrentPage = Children[0]; break;
			}
		}
		private async Task ConnectToRealmDb()
		{
			//await ViewModel.RealmDbConnectionCommand();
		}

	}
}


