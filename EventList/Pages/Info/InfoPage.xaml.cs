using System;
using System.Collections.Generic;
using EventList.Models;
using EventList.Pages.CommonEvents;
using EventList.Pages.Feedback;
using EventList.Pages.Sessions;
using EventList.Pages.Sponsors;
using EventList.ViewModels;
using Xamarin.Forms;

namespace EventList.Pages.Info
{
    public partial class InfoPage : ContentPage
    {
        InfoViewModel vm;

        public InfoPage()
        {
            InitializeComponent();

            Util.CommonUtility.VisibleNavigationBar(this);

            BindingContext = vm = new InfoViewModel(Navigation);

            lvPoiCategories.ItemTapped += (sender, e) => lvPoiCategories.SelectedItem = null;
            lvPoiCategories.ItemSelected +=async (sender, e) =>
            {
                var category = lvPoiCategories.SelectedItem as POICategory;
                if (category == null)
                    return;
                vm.SelectedPOICategory = category;
                vm.LoadPOISubCategoryCommand.Execute(null);
                if (category.Title.Equals("Venue"))
                {
                    if (vm.POISubCategories != null && vm.POISubCategories.Count > 0)
                    {
                       string description= vm.POISubCategories[0].Description;
                       Venue venue = Newtonsoft.Json.JsonConvert.DeserializeObject<Venue>(description); 
                        await Navigation.PushAsync(new VenuePage(venue));
                    }
                }
                else if (category.Title.Equals("Conference Floor Maps"))
                {
                    if (vm.POISubCategories != null && vm.POISubCategories.Count > 0)
                    {
                        string description = vm.POISubCategories[0].Description;
                        ConferenceFloor floor = Newtonsoft.Json.JsonConvert.DeserializeObject<ConferenceFloor>(description);
                        await Navigation.PushAsync(new FloorMapsPage(floor));
                    }
                }
                else if (category.Title.Equals("Code of Conduct"))
                {
                    if (vm.POISubCategories != null && vm.POISubCategories.Count > 0)
                    {
                        string description = vm.POISubCategories[0].Description;
                        await Navigation.PushAsync(new CodeOfConductPage(description));
                    }
                }
                else if (category.Title.Equals("Wi-Fi Information"))
                {
                    if (vm.POISubCategories != null && vm.POISubCategories.Count > 0)
                    {
                        string description = vm.POISubCategories[0].Description;
                        Wifi wifi = Newtonsoft.Json.JsonConvert.DeserializeObject<Wifi>(description);
                        await Navigation.PushAsync(new WifiInformationPage(wifi));
                    }
                }
                else if (category.Title.Equals("Sponsors"))
                {
                    await Navigation.PushAsync(new SponsorsPage());
                }
                else if (category.Title.Equals("Evaluations"))
                {
                    await Navigation.PushAsync(new EvaluationPage());
                }
                else
                {
                    await Navigation.PushAsync(new POISubCategoryPage(vm.POISubCategoriesGrouped));
                }              
            };
			
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            vm.LoadPOIDataCommand.Execute(null);
            vm.LoadEvaluationCommand.Execute(null);
        }
	}
}

