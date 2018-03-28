using System;
using System.Threading.Tasks;

namespace EventList
{
	public interface IPushNotifications
	{
		Task<bool> RegisterForNotifications();

		bool IsRegistered { get; }

		void OpenSettings();
	}
}
