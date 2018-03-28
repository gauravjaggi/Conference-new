using System;
using System.Collections.Generic;
using Xamarin.Forms;
using EventList.Pages.Home;
using System.Threading.Tasks;
using EventList.Pages.Sessions;
using EventList.Models;
using EventList.Pages.MiniHacks;
using EventList.Pages.Events;
using FormsToolkit;

namespace EventList.Pages.Android
{
	public class RootPageAndroid : MasterDetailPage
	{
        Dictionary<int, NavigationPage> pages;
		bool isRunning = false;
		public RootPageAndroid()
		{
			pages = new Dictionary<int, NavigationPage>();
			Master = new MenuPage(this);

			pages.Add(0, new NavigationPage(new FeedsPage()));

			Detail = pages[0];			
		}



		public async Task NavigateAsync(int menuId)
		{
			NavigationPage newPage = null;
			if (!pages.ContainsKey(menuId))
			{
				//only cache specific pages
				switch (menuId)
				{
					case (int)AppPage.Feed: //Feed
						pages.Add(menuId, new NavigationPage(new FeedsPage()));
						break;
					case (int)AppPage.Sessions://sessions
						pages.Add(menuId, new NavigationPage(new SessionsPage()));
						break;
					case (int)AppPage.Events://events
                        pages.Add(menuId, new NavigationPage(new EventListPage()));
						break;
					case (int)AppPage.MiniHacks://Mini-Hacks
						newPage = new NavigationPage(new MiniHacksPage());
						break;										
				}
			}

			if (newPage == null)
				newPage = pages[menuId];

			if (newPage == null)
				return;

			//if we are on the same tab and pressed it again.
			if (Detail == newPage)
			{
				await newPage.Navigation.PopToRootAsync();
			}

			Detail = newPage;
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();


			if (Settings.Current.FirstRun)
			{
				MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
			}

			isRunning = true;		

		}		

	}
}


