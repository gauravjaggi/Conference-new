using System;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Preferences;
using Android.Widget;
using EventList.Droid.Services;
using EventList.Interfaces;
using EventList.Models;
using EventList.ViewModels;
using Xamarin.Forms;

[assembly: Dependency(typeof(InputAlertService))]
namespace EventList.Droid.Services
{
    public class InputAlertService:IInputAlert
    {
        EditText et;
        FeedViewModel _vm;
        private ISharedPreferences _sharedPref = PreferenceManager.GetDefaultSharedPreferences(Android.App.Application.Context);
        public InputAlertService()
        {
            
        }

        public string GetSavedRealmSetting()
        {
            return _sharedPref.GetString("realm",string.Empty);
        }

        async public Task Show(FeedViewModel vm)
        {
            try
            {
                _vm = vm;
                Activity activity = Forms.Context as Activity;
                et = new EditText(activity);
                using (var builder = new AlertDialog.Builder(activity))
                {
                    builder.SetTitle("Enter Code");
                    builder.SetView(et);
                    builder.SetPositiveButton("OK", (senderAlert, args) => {
                        ButtonPressed();
                    });
                    var myCustomDialog = builder.Create();
                    myCustomDialog.Show();
                }             
            }
            catch(Exception x )
            {
                
            }

        }
        public async void ButtonPressed()
        {
            if (!string.IsNullOrEmpty(et.Text))
            {
                var result = await _vm.GetRealmSettings(et.Text);
                if (result != null && result.Count > 0)
                {
                    RealmSetting s = null;
                    if (result.Any(x => x.Code.Equals(et.Text)))
                    {
                        s = result.Where(x => x.Code.Equals(et.Text)).FirstOrDefault();
                    }
                    else
                    {
                        s = result.Where(x => x.Code.Equals("default")).FirstOrDefault();
                    }
                    App.CurrentRealmSetting = s;
                    _sharedPref.Edit().PutString("realm", s.ToJson()).Commit();
                    _vm.RealmDbConnectionCommand.Execute(null);
                }
            }
        }
    }
}
