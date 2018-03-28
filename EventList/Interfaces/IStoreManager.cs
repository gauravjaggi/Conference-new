using System;
using EventList.Interfaces;

namespace EventList
{
	public interface IStoreManager
	{
		bool IsInitialized { get; }
		ICategoryStore CategoryStore { get; }
		IFavoriteStore FavoriteStore { get; }
		IFeedbackStore FeedbackStore { get; }
		ISessionStore SessionStore { get; }
		ISpeakerStore SpeakerStore { get; }
		ISponsorStore SponsorStore { get; }
		IEventStore EventStore { get; }
		IMiniHacksStore MiniHacksStore { get; }
		INotificationStore NotificationStore { get; }
	}
}
