using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using EventList.Extensions;
using EventList.Models;
using EventList.Pages.CommonEvents;
using FormsToolkit;
using MvvmHelpers;
using Xamarin.Forms;

namespace EventList
{
    public class InfoViewModel : SettingsViewModel
    {
        public ObservableRangeCollection<Grouping<string, MenuItem>> MenuItems { get; }
        public ObservableRangeCollection<MenuItem> InfoItems { get; } = new ObservableRangeCollection<MenuItem>();

        public ObservableRangeCollection<POICategory> POICategories { get; } = new ObservableRangeCollection<POICategory>();
        public ObservableRangeCollection<Grouping<string, POICategory>> CategoriesGrouped { get; } = new ObservableRangeCollection<Grouping<string, POICategory>>();

        public ObservableRangeCollection<POISubCategory> POISubCategories { get; set; } = new ObservableRangeCollection<POISubCategory>();
        public ObservableRangeCollection<Grouping<string, POISubCategory>> POISubCategoriesGrouped { get; } = new ObservableRangeCollection<Grouping<string, POISubCategory>>();

        public ObservableRangeCollection<POIItem> POIItems { get; set; } = new ObservableRangeCollection<POIItem>();

        public INavigation _navigation { get; set; }
        public InfoViewModel(INavigation navigation) : base(navigation)
        {
            _navigation = navigation;
            try
            {
                AboutItems.Clear();
                AboutItems.Add(new MenuItem { Name = "About this app", Icon= "About.png" });

                InfoItems.AddRange(new[]{
                    new MenuItem { Name = "Sponsors", Icon = "icon_venue.png", Parameter="sponsors"},
                    new MenuItem { Name = "Evaluations", Icon = "evaluation.png", Parameter="evaluations"},
                    new MenuItem { Name = "Venue", Icon = "location.png", Parameter = "venue"},
                    new MenuItem { Name = "Conference Floor Maps", Icon = "map.png", Parameter = "floor-maps"},
                    new MenuItem { Name = "Code of Conduct", Icon = "Code.png", Parameter="code-of-conduct" },
                    new MenuItem { Name = "Wi-Fi Information", Icon = "wifi.png", Parameter="wi-fi" },
            });
            }
            catch (Exception ex)
            {

            }
        }

        ICommand loadEvaluationCommand;
        public ICommand LoadEvaluationCommand =>
            loadEvaluationCommand ?? (loadEvaluationCommand = new Command(async () => await ExecuteLoadEvaluationCommand()));

        async Task ExecuteLoadEvaluationCommand()
        {
            try
            {
                var feedbacks = App._Realm.All<Feedback>();
                List<Feedback> eventfeedbackslist = feedbacks.Where(x => x.EventType == "Info").ToList();
                if (eventfeedbackslist.Count() > 0)
                {
                    MenuItem evalItem = InfoItems.Where(x => x.Name.Equals("Evaluations")).FirstOrDefault();
                    if (evalItem != null)
                        InfoItems.Remove(evalItem);
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }
        ICommand loadPOIDataCommand;
        public ICommand LoadPOIDataCommand =>
        loadPOIDataCommand ?? (loadPOIDataCommand = new Command(async () => await ExecuteLoadPOIDataCommandAsync()));

        async Task ExecuteLoadPOIDataCommandAsync()
        {
            try
            {
                List<POICategory> categories = new List<POICategory>();
                var poicategories = App._Realm.All<POICategory>();
                if (poicategories.Count().Equals(0))
                {
                    
                    categories = await APIServices.GetPOICategories();

                    foreach (POICategory item in categories)
                    {
                       var cats = App._Realm.All<POICategory>();
                        if (!cats.Any(x => x.CategoryID==item.CategoryID))
                        {
                            App._Realm.Write(() =>
                            {
                                App._Realm.Add(item);
                            });
                        }
                    }
                    POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("info")));
                }
                else
                {
                    foreach (var item in poicategories)
                    {
                        if (!categories.Any(x => x.CategoryID == item.CategoryID))
                        {
                            POICategory pOICategory = new POICategory()
                            {
                                CategoryID = item.CategoryID,
                                Tab = item.Tab,
                                TableSection = item.TableSection,
                                Title = item.Title,
                                SubTitle = item.SubTitle,
                            };
                            if (pOICategory.Title.Equals("Venue"))
                            {
                                pOICategory.Image = "location.png";
                            }
                            else if (pOICategory.Title.Equals("Conference Floor Maps"))
                            {
                                pOICategory.Image = "map.png";
                            }
                            else if (pOICategory.Title.Equals("Code of Conduct"))
                            {
                                pOICategory.Image = "Code.png";
                            }
                            else if (pOICategory.Title.Equals("Wi-Fi Information"))
                            {
                                pOICategory.Image = "wifi.png";
                            }
                            else if (pOICategory.Title.Equals("About this app"))
                            {
                                pOICategory.Image = "About.png";
                            }
                            else if (pOICategory.Title.Equals("Evaluations"))
                            {
                                pOICategory.Image = "evaluation.png";
                            }
                            categories.Add(pOICategory);
                        }
                    }
                    POICategories.ReplaceRange(categories.Where(x => x.Tab.Equals("info")));

                }
                CategoriesGrouped.ReplaceRange(POICategories.GroupBySection());
            }
            catch (Exception ex)
            {

            }
        }
        public POICategory SelectedPOICategory { get; set; }

        ICommand loadPOISubCategoryCommand;
        public ICommand LoadPOISubCategoryCommand =>
        loadPOISubCategoryCommand ?? (loadPOISubCategoryCommand = new Command(async () => await ExecuteLoadPOISubCategoryCommandAsync()));

        async Task ExecuteLoadPOISubCategoryCommandAsync()
        {
            try
            {
                var poisubcategories = App._Realm.All<POISubCategory>();
                if (poisubcategories.Count().Equals(0))
                {
                    var subCategories = await APIServices.GetPOISubCategories();
                    foreach (POISubCategory item in subCategories)
                    {
                        var subcats = App._Realm.All<POISubCategory>();
                        if (!subcats.Any(x => x.SubCategoryID==item.SubCategoryID))
                        {
                            App._Realm.Write(() =>
                            {
                                App._Realm.Add(item);
                            });
                        }
                    }
                    POISubCategories = new ObservableRangeCollection<POISubCategory>();
                    foreach (var item in subCategories)
                    {
                        if (item.CategoryId == SelectedPOICategory.CategoryID)
                        {
                            POISubCategories.Add((item));
                        }
                    }
                }
                else
                {
                    POISubCategories = new ObservableRangeCollection<POISubCategory>();
                    foreach (var item in poisubcategories)
                    {
                        if (item.CategoryId == SelectedPOICategory.CategoryID)
                        {
                            POISubCategories.Add((item));
                        }
                    }
                }
                POISubCategoriesGrouped.ReplaceRange(POISubCategories.GroupBySection());

            }
            catch (Exception ex)
            {

            }
        }
        public POISubCategory SelectedPOISubCategory { get; set; }

        ICommand loadPOIItemCommand;
        public ICommand LoadPOIItemCommand =>
        loadPOIItemCommand ?? (loadPOIItemCommand = new Command(async () => await ExecuteLoadPOIItemCommandAsync()));

        async Task ExecuteLoadPOIItemCommandAsync()
        {
            try
            {
                var poiitems = App._Realm.All<POIItem>();
                if (poiitems.Count().Equals(0))
                {
                    var items = await APIServices.GetPOIItems();
                    foreach (POIItem item in items)
                    {
                        App._Realm.Write(() =>
                        {
                            App._Realm.Add(item);
                        });
                    }
                    POIItems = new ObservableRangeCollection<POIItem>();
                    foreach (var item in items)
                    {
                        if (item.SubCategoryId == SelectedPOISubCategory.SubCategoryID)
                        {
                            POIItems.Add((item));
                        }
                    }
                }
                else
                {
                    POIItems = new ObservableRangeCollection<POIItem>();
                    foreach (var item in poiitems)
                    {
                        if (item.SubCategoryId == SelectedPOISubCategory.SubCategoryID)
                        {
                            POIItems.Add((item));
                        }
                    }
                }
                await _navigation.PushAsync(new POIItemPage(POIItems, SelectedPOISubCategory.Title));
            }
            catch (Exception ex)
            {

            }
        }
    }
}

        
