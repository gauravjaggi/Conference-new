﻿using System;
using System.Threading.Tasks;
using EventList.Interfaces;
using EventList.Models;

namespace EventList.Store
{
	public class FeedbackStore : BaseStore<Feedback>, IFeedbackStore
	{
		public Task<bool> LeftFeedback(Session session)
		{
			return Task.FromResult(Settings.LeftFeedback(session.Id));
		}

		public async Task DropFeedback()
		{
			await Settings.ClearFeedback();
		}

		public override Task<bool> InsertAsync(Feedback item)
		{
			//Settings.LeaveFeedback(item.SessionId, true);
			return Task.FromResult(true);
		}

		public override Task<bool> RemoveAsync(Feedback item)
		{
			//Settings.LeaveFeedback(item.SessionId, false);
			return Task.FromResult(true);
		}
	}
}

