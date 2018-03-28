using System;
using EventList.Models;
using System.Threading.Tasks;
namespace EventList.Interfaces
{
	public interface INotificationStore : IBaseStore<Notification>
	{
		Task<Notification> GetLatestNotification();
	}
}
