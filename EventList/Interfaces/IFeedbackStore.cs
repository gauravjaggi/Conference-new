using System;
using EventList.Models;
using System.Threading.Tasks;
namespace EventList.Interfaces
{
	public interface IFeedbackStore : IBaseStore<Feedback>
	{
		Task<bool> LeftFeedback(Session session);
		Task DropFeedback();
	}
}
