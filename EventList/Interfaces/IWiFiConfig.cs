using System;
namespace EventList.Interfaces
{
	public interface IWiFiConfig
	{
		bool ConfigureWiFi(string ssid, string password);
		bool IsConfigured(string ssid);
		bool IsWiFiOn();
	}
}
