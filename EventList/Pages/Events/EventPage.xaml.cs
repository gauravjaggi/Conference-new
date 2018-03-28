using EventList.Helpers;
using EventList.Pages.CommonEvents;
using EventList.Pages.Favorites;
using EventList.Pages.MiniHacks;
using EventList.Pages.Schedule;
using EventList.Pages.Sessions;
using EventList.Pages.Speakers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EventList.Pages.Events
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventPage : ContentPage
    {
        public EventPage()
        {
            InitializeComponent();
        }

        private async void ClickedSchedule_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new SchedulePage());
        }
        private async void ClickedFavorites_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new FavoritesPage());
        }
        private async void ClickedSession_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new SessionsPage());
        }
        private async void ClickedFeaturedEvent_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new EventListPage());
        }
        private async void ClickedMiniHacks_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new MiniHacksPage());
        }
        private async void ClickedSpeakers_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new SpeakersPage());
        }
        private async void ClickedAttendees_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new AttendeesPage());
        }
        private async void ClickedMyNetworks_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new MyNetworks());
        }
        private async void ClickedOtherImage_Tapped(object sender, EventArgs e)
        {
            await NavigationService.PushAsync(Navigation, new CommonEventListPage());
        }
    }
}