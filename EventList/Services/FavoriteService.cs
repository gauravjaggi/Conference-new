using System;
using EventList.Models;
using FormsToolkit;
using System.Threading.Tasks;
using Xamarin.Forms;
using EventList.Interfaces;
using System.Linq;
namespace EventList.Services
{
	public class FavoriteService
	{
		Session sessionQueued;
		public FavoriteService()
		{
			MessagingService.Current.Subscribe(MessageKeys.LoggedIn, async (s) =>
				{
					if (sessionQueued == null)
						return;

					await ToggleFavorite(sessionQueued);
				});
		}
		public async Task<bool> ToggleFavorite(Session session)
		{
			//if (!Settings.Current.IsLoggedIn)
			//{
			//	sessionQueued = session;				
			//	MessagingService.Current.SendMessage(MessageKeys.NavigateLogin);
			//	return false;
			//}

			sessionQueued = null;

			var store = DependencyService.Get<IFavoriteStore>();
			session.IsFavorite = !session.IsFavorite;//switch first so UI updates :)
			if (!session.IsFavorite)
			{		
                				var items = await store.GetItemsAsync();
                foreach (var item in items.Where(s => s.Id == session.Id))
				{
					await store.RemoveAsync(item);
				}
			}
			else
			{				
                await store.InsertAsync(new Favorite { Id = session.Id });
			}

			Settings.Current.LastFavoriteTime = DateTime.UtcNow;
			return true;
		}
	}
}

