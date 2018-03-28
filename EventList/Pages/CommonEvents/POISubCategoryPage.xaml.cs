using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.ViewModels;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList.Pages.CommonEvents
{
    public partial class POISubCategoryPage : ContentPage
    {
        FeedViewModel ViewModel => vm ?? (vm = BindingContext as FeedViewModel);
        FeedViewModel vm;
        public POISubCategoryPage(ObservableRangeCollection<Grouping<string, POISubCategory>> subcategories)
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

            BindingContext = vm = new FeedViewModel(this.Navigation);
            this.lvPoiSubCategories.ItemsSource = subcategories;
            lvPoiSubCategories.ItemTapped += (sender, e) => lvPoiSubCategories.SelectedItem = null;
            lvPoiSubCategories.ItemSelected +=  (sender, e) =>
            {
                var subcategory = lvPoiSubCategories.SelectedItem as POISubCategory;
                if (subcategory == null)
                    return;
                
                vm.SelectedPOISubCategory = subcategory;
                vm.LoadPOIItemCommand.Execute(null);
                lvPoiSubCategories.SelectedItem = null;
            };
        }
    }
}
