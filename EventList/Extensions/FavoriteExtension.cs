using System;
using System.Collections.Generic;
using MvvmHelpers;
using System.Linq;
using EventList.ViewModels;

namespace EventList
{
    public static class FavoriteExtension
    {
		public static IEnumerable<Grouping<string, FavoriteItemsViewModel>> GroupByUser(this IEnumerable<FavoriteItemsViewModel> favorite)
		{
			return from e in favorite
                   orderby e.DeviceUser
				   group e by e.DeviceUser
                   into favoritegroup
				   select new Grouping<string, FavoriteItemsViewModel>(favoritegroup.Key, favoritegroup);
		}

	}
}
