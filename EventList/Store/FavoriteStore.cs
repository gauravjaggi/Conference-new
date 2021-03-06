﻿using System;
using System.Threading.Tasks;
using EventList.Interfaces;
using EventList.Models;

namespace EventList.Store
{
	public class FavoriteStore : BaseStore<Favorite>, IFavoriteStore
	{
		public Task<bool> IsFavorite(string sessionId)
		{
			return Task.FromResult(Settings.IsFavorite(sessionId));
		}

		public override Task<bool> InsertAsync(Favorite item)
		{
            Settings.SetFavorite(item.Id, true);
			return Task.FromResult(true);
		}

		public override Task<bool> RemoveAsync(Favorite item)
		{
            Settings.SetFavorite(item.Id, false);
			return Task.FromResult(true);
		}

		public async Task DropFavorites()
		{
			await Settings.ClearFavorites();
		}
	}
}

