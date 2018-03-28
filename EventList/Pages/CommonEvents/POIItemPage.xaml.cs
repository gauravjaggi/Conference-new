using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.ViewModels;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList.Pages.CommonEvents
{
    public partial class POIItemPage : ContentPage
    {
        FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);
        FeedViewModel vm;
        public POIItemPage(ObservableRangeCollection<POIItem> items,string title)
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

            BindingContext = vm = new FeedViewModel(this.Navigation);
            this.lvItems.ItemsSource = items;

            this.Title = title;

            this.lvItems.ItemsSource = items;
        }
       
    }
}
