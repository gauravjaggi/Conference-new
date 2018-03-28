using System;
using System.Windows.Input;
using System.Threading.Tasks;
using FormsToolkit;
using Plugin.Connectivity;
using Plugin.Share;
using Xamarin.Forms;
using MvvmHelpers;
using Humanizer;

namespace EventList
{
    public class SettingsViewModel : ViewModelBase
    {

        public ObservableRangeCollection<MenuItem> AboutItems { get; } = new ObservableRangeCollection<MenuItem>();
        public ObservableRangeCollection<MenuItem> TechnologyItems { get; } = new ObservableRangeCollection<MenuItem>();

        public SettingsViewModel(INavigation navigation) : base(navigation)
        {
            AboutItems.AddRange(new[]
                {
                    new MenuItem { Name = "Created by Xamarin with <3", Command=LaunchBrowserCommand, Parameter="http://www.xamarin.com" },
                    new MenuItem { Name = "Open source on GitHub!", Command=LaunchBrowserCommand, Parameter="http://tiny.cc/app-evolve"},
                    new MenuItem { Name = "Terms of Use", Command=LaunchBrowserCommand, Parameter="https://store.xamarin.com/terms"},
                    new MenuItem { Name = "Privacy Policy", Command=LaunchBrowserCommand, Parameter="http://xamarin.com/privacy"},
                    new MenuItem { Name = "Open Source Notice", Command=LaunchBrowserCommand, Parameter="http://tiny.cc/app-evolve-osn"}
                });

            TechnologyItems.AddRange(new[]
                {
                    new MenuItem { Name = "Azure Mobile Apps", Command=LaunchBrowserCommand, Parameter="https://github.com/Azure/azure-mobile-apps-net-client/" },
                    new MenuItem { Name = "Censored", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Censored"},
                    new MenuItem { Name = "Calendar Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/TheAlmightyBob/Calendars"},
                    new MenuItem { Name = "Connectivity Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Connectivity"},
                    new MenuItem { Name = "Embedded Resource Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/JosephHill/EmbeddedResourcePlugin"},
                    new MenuItem { Name = "External Maps Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ExternalMaps"},
                    new MenuItem { Name = "Humanizer", Command=LaunchBrowserCommand, Parameter="https://github.com/Humanizr/Humanizer"},
                    new MenuItem { Name = "Image Circles", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/ImageCircle"},
                    new MenuItem { Name = "Json.NET", Command=LaunchBrowserCommand, Parameter="https://github.com/JamesNK/Newtonsoft.Json"},
                    new MenuItem { Name = "LinqToTwitter", Command=LaunchBrowserCommand, Parameter="https://github.com/JoeMayo/LinqToTwitter"},
                    new MenuItem { Name = "Messaging Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/cjlotz/Xamarin.Plugins"},
                    new MenuItem { Name = "Mvvm Helpers", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/mvvm-helpers"},
                    new MenuItem { Name = "Noda Time", Command=LaunchBrowserCommand, Parameter="https://github.com/nodatime/nodatime"},
                    new MenuItem { Name = "Permissions Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Permissions"},
                    new MenuItem { Name = "PCL Storage", Command=LaunchBrowserCommand, Parameter="https://github.com/dsplaisted/PCLStorage"},
                    new MenuItem { Name = "Pull to Refresh Layout", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Xamarin.Forms-PullToRefreshLayout"},
                    new MenuItem { Name = "Settings Plugin", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/Xamarin.Plugins/tree/master/Settings"},
                    new MenuItem { Name = "Toolkit for Xamarin.Forms", Command=LaunchBrowserCommand, Parameter="https://github.com/jamesmontemagno/xamarin.forms-toolkit"},
                    new MenuItem { Name = "Xamarin.Forms", Command=LaunchBrowserCommand, Parameter="http://xamarin.com/forms"},
                    new MenuItem { Name = "Xamarin Insights", Command=LaunchBrowserCommand, Parameter="http://xamarin.com/insights"},
                    new MenuItem { Name = "ZXing.Net Mobile", Command=LaunchBrowserCommand, Parameter="https://github.com/Redth/ZXing.Net.Mobile"}
                    });
        }


    }
}
