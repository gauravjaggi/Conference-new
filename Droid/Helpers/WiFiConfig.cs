using System;
using Xamarin.Forms;
using Android.App;
using Plugin.CurrentActivity;
using Android.Widget;
using EventList.Droid.Helpers;
using EventList.Interfaces;
using Android.Content;
using Android.Net.Wifi;

[assembly: Dependency(typeof(WiFiConfig))]
namespace EventList.Droid.Helpers
{
	public class WiFiConfig : IWiFiConfig
	{

		#region IWiFiConfig implementation

		public bool ConfigureWiFi(string ssid, string password)
		{
			try
			{
				var wifiConfig = GetWifiConfig(ssid, password);
				var wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;

				if (wifiManager == null)
					return false;

				var netId = wifiManager.AddNetwork(wifiConfig);
				if (netId != -1)
				{
					wifiManager.EnableNetwork(netId, false);
					var result = wifiManager.SaveConfiguration();
					if (!result)
					{						
						return false;
					}
				}
				else
				{					
					return false;
				}
			}
			catch (Exception ex)
			{
				return false;
			}

			return true;
		}

		public bool IsConfigured(string ssid)
		{
			var wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;
			if (wifiManager == null)
				return false;

			var finalSsid = string.Format("\"{0}\"", ssid);
			foreach (var id in wifiManager.ConfiguredNetworks)
			{
				if (id == null)
					continue;


				if (string.IsNullOrWhiteSpace(id.Ssid))
					continue;

				if (id.Ssid.Equals(finalSsid, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}

			return false;
		}

		public bool IsWiFiOn()
		{
			var wifiManager = Forms.Context.GetSystemService(Context.WifiService) as WifiManager;

			return wifiManager?.IsWifiEnabled ?? false;
		}

		#endregion

		// Must be in double quotes to tell system this is an ASCII SSID and passphrase.
		private WifiConfiguration GetWifiConfig(string ssid, string password)
		{

			if (string.IsNullOrWhiteSpace(password))
				return new WifiConfiguration
				{
					Ssid = Java.Lang.String.Format("\"%s\"", ssid)
				};

			return new WifiConfiguration
			{
				Ssid = Java.Lang.String.Format("\"%s\"", ssid),
				PreSharedKey = Java.Lang.String.Format("\"%s\"", password)
			};
		}
	}
}

