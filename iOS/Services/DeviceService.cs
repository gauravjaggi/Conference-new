using System;
using EventList.Helpers;
using EventList.iOS.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceService))]
namespace EventList.iOS.Services
{
    public class DeviceService:IDeviceService
    {
        public string GetDeviceName()
        {
          return UIDevice.CurrentDevice.Name;
        }
    }
}
