using System;
using EventList.Models;
using System.Threading.Tasks;
namespace EventList.Interfaces
{
	public interface IFavoriteStore : IBaseStore<Favorite>
	{
		Task<bool> IsFavorite(string sessionId);
		Task DropFavorites();
	}
}
