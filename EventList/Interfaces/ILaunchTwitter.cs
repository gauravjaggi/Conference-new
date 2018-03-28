using System;
namespace EventList.Interfaces
{
	public interface ILaunchTwitter
	{
		bool OpenUserName(string username);
		bool OpenStatus(string statusId);
	}
}
