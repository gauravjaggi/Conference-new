using System;
using EventList.Models;
using Xamarin.Forms;

namespace EventList.ViewModels
{
    public class FavoriteItemsViewModel:ViewModelBase
    {
        public FavoriteItemsViewModel()
        {
        }
        public string DeviceUser { get; set; }

        public Session FavoriteSession { get; set; }
      
        public FeaturedEvent FavoriteEvent { get; set; }
    }
}
