using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventList.Models;
namespace EventList.Interfaces
{
    public interface ISessionStore : IBaseStore<Session>
	{
		Task<IEnumerable<Session>> GetSpeakerSessionsAsync(string speakerId);
		Task<IEnumerable<Session>> GetNextSessions();
		Task<Session> GetAppIndexSession(string id);
	}
}


