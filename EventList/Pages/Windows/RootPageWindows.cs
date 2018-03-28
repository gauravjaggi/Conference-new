using System;
using System.Collections.Generic;
using Xamarin.Forms;
using EventList.Models;
using EventList.Pages.Events;
using EventList.Pages.MiniHacks;
using EventList.Pages.Home;
using EventList.Pages.Sessions;
using FormsToolkit;
using System.Collections.ObjectModel;
namespace EventList.Pages.Windows
{
    public class RootPageWindows : MasterDetailPage
    {
        Dictionary<AppPage, Page> pages;
        MenuPageUWP menu;
        public static bool IsDesktop { get; set; }
        public RootPageWindows()
        {
            //MasterBehavior = MasterBehavior.Popover;
            pages = new Dictionary<AppPage, Page>();

            var items = new ObservableCollection<MenuItem>();

            items.Add(new MenuItem() { Name = "Evolve Feed", Page = AppPage.Feed });
            items.Add(new MenuItem { Name = "Sessions", Page = AppPage.Sessions });
            items.Add(new MenuItem { Name = "Events", Page = AppPage.Events });
            items.Add(new MenuItem { Name = "Mini-Hacks", Page = AppPage.MiniHacks });

            menu = new MenuPageUWP();
            menu.MenuList.ItemsSource = items;


            menu.MenuList.ItemSelected += (sender, args) =>
            {
                if (menu.MenuList.SelectedItem == null)
                    return;

                Device.BeginInvokeOnMainThread(() =>
                {
                    NavigateAsync(((MenuItem)menu.MenuList.SelectedItem).Page);
                    if (!IsDesktop)
                        IsPresented = false;
                });
            };

            Master = menu;
            NavigateAsync((int)AppPage.Feed);
            Title = "Xamarin Evolve";
        }



        public void NavigateAsync(AppPage menuId)
        {
            Page newPage = null;
            if (!pages.ContainsKey(menuId))
            {
                //only cache specific pages
                switch (menuId)
                {
                    case AppPage.Feed: //Feed
                        pages.Add(menuId, new NavigationPage(new FeedsPage()));
                        break;
                    case AppPage.Sessions://sessions
                        pages.Add(menuId, new NavigationPage(new SessionsPage()));
                        break;
                    case AppPage.Events://events
                        pages.Add(menuId, new NavigationPage(new EventListPage()));
                        break;
                    case AppPage.MiniHacks://Mini-Hacks
                        newPage = new NavigationPage(new MiniHacksPage());
                        break;                    
                }
            }

            if (newPage == null)
                newPage = pages[menuId];

            if (newPage == null)
                return;

            Detail = newPage;
            //await Navigation.PushAsync(newPage);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();


            if (Settings.Current.FirstRun)
            {
                MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
            }
        }

    }
}


