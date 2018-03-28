using System;
using System.Linq;
using System.Threading.Tasks;
using EventList.Interfaces;
using EventList.iOS.Services;
using EventList.Models;
using EventList.ViewModels;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(InputAlertService))]
namespace EventList.iOS.Services
{
    public class InputAlertService : IInputAlert
    {
        public string GetSavedRealmSetting()
        {
            var plist = NSUserDefaults.StandardUserDefaults;
            return plist.StringForKey("realm");
        }

        async public Task Show(FeedViewModel vm)
        {
            var alert = new UIAlertView();
            alert.AddButton("Ok");
            alert.Title = "Enter Code";
            alert.AlertViewStyle = UIAlertViewStyle.SecureTextInput;
            alert.Show();

            alert.Clicked+=async (sender, e) => {
                string text=alert.GetTextField(0).Text;
                RealmSetting s = null;
                if (!string.IsNullOrEmpty(text))
                {
                    var result = await vm.GetRealmSettings(text);
                    if (result != null && result.Count > 0)
                    {
                        if (result.Any(x => x.Code.Equals(text)))
                        {
                            s = result.Where(x => x.Code.Equals(text)).FirstOrDefault();
                        }
                        else
                        {
                            s = result.Where(x => x.Code.Equals("default")).FirstOrDefault();
                        }
                        App.CurrentRealmSetting = s;
                        NSUserDefaults.StandardUserDefaults.SetString(s.ToJson(), "realm");
                        vm.RealmDbConnectionCommand.Execute(null);
                    }
                }
            };
        }
    }
}
