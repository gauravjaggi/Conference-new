using System;
using EventList.Droid.Services;
using EventList.Helpers;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceService))]
namespace EventList.Droid.Services
{
    public class DeviceService:IDeviceService
    {
       public string GetDeviceName()
        {
            return Android.OS.Build.Model;
        }
    }
}
