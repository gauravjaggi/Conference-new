using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
//using EventList.Extensions;
using EventList.Models;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList.ViewModels
{
    public class FavoriteViewModel :ViewModelBase
    {
  //      public ObservableRangeCollection<Favorite> Favorites { get; set; }
		//public ObservableRangeCollection<Grouping<string, Favorite>> FavoritesGrouped { get; } = new ObservableRangeCollection<Grouping<string, Favorite>>();

        public ObservableRangeCollection<FavoriteItemsViewModel> Favorites { get; set; }// = new ObservableRangeCollection<Favorite>();
		public ObservableRangeCollection<Grouping<string, FavoriteItemsViewModel>> FavoritesGrouped { get; } = new ObservableRangeCollection<Grouping<string, FavoriteItemsViewModel>>();

        public FavoriteViewModel(INavigation navigation) : base(navigation)
        {
            Title = "Favorites";
        }

		ICommand loadFavoritesCommand;
        public ICommand LoadFavoritesCommand => loadFavoritesCommand ?? (loadFavoritesCommand = new Command(async () => await ExecuteLoadFavoritesAsync()));

		async Task<bool> ExecuteLoadFavoritesAsync()
		{
			if (IsBusy)
				return false;

			try
			{
				IsBusy = true;
#if DEBUG
				await Task.Delay(1000);
#endif
                Favorites = new ObservableRangeCollection<FavoriteItemsViewModel>();
				List<Session> Sessions = new List<Session>();

                var sessions = App._Realm.All<Session>();
				var speakers = App._Realm.All<Models.Speaker>();

				var featuredevents = App._Realm.All<FeaturedEvent>();
				var favorites = App._Realm.All<Favorite>();

                if (sessions.Count() == 0 || featuredevents.Count() == 0 || favorites.Count() == 0)
                    return false;
                
                foreach(var session in sessions)
                {
                    session.SessionSpeakers = new List<Speaker>();
                    foreach(var speaker in speakers)
                    {
                        if(speaker.SessionId.Equals(session.Id))
                        {
                            session.SessionSpeakers.Add(speaker);
                        }
                    }
                    Sessions.Add(session);
                }
               

				if (favorites.Count() > 0)
				{
					foreach (var favorite in favorites)
					{
                        var item=new FavoriteItemsViewModel(){
                            DeviceUser=favorite.DeviceUser
                        };
                        if (favorite.EventType.Equals("Event"))
                        {
                            item.FavoriteEvent = featuredevents.Where(x => x.Id == favorite.Id).FirstOrDefault();
                            item.FavoriteSession = null;
                        }
                        else if(favorite.EventType.Equals("Session"))
						{
                            item.FavoriteSession = Sessions.Where(x => x.Id == favorite.Id).FirstOrDefault();
							item.FavoriteEvent = null;
						}
                        Favorites.Add(item);
					}
                    GroupFavorites();
				}
					
			}
			catch (Exception ex)
			{
				//MessagingService.Current.SendMessage(MessageKeys.Error, ex);
			}
			finally
			{
				IsBusy = false;
			}
			return true;
		}

		void GroupFavorites()
		{
			FavoritesGrouped.ReplaceRange(Favorites.GroupByUser());
		}
    }
}
