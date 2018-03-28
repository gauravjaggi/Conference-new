﻿using System;
using System.Collections.Generic;
using System.Linq;
using EventList.Models;
using MvvmHelpers;

namespace EventList
{
	public static class SessionStateExtensions
	{
		

		public static string GetIndexName(this Session e)
		{
			if (!e.StartTime.HasValue || !e.EndTime.HasValue || e.StartTime.Value.UtcDateTime.IsTBA())
				return "To be announced";

			var start = e.StartTime.Value.UtcDateTime.ToEasternTimeZone();


			var startString = start.ToString("t");
			var end = e.EndTime.Value.UtcDateTime.ToEasternTimeZone();
			var endString = end.ToString("t");

			var day = start.DayOfWeek.ToString();
			var monthDay = start.ToString("M");
			return $"{day}, {monthDay}, {startString}–{endString}";
		}

		public static string GetSortName(this Session session)
		{

			if (!session.StartTime.HasValue || !session.EndTime.HasValue || session.StartTime.Value.UtcDateTime.IsTBA())
				return "To be announced";

			var start = session.StartTime.Value.UtcDateTime.ToEasternTimeZone();
			var startString = start.ToString("t");

			if (DateTime.Today.Year == start.Year)
			{
				if (DateTime.Today.DayOfYear == start.DayOfYear)
					return $"Today {startString}";

				if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
					return $"Tomorrow {startString}";
			}
			var day = start.ToString("M");
			return $"{day}, {startString}";
		}

		public static string GetDisplayName(this Session session)
		{
			if (!session.StartTime.HasValue || !session.EndTime.HasValue || session.StartTime.Value.UtcDateTime.IsTBA())
				return "TBA";

			var start = session.StartTime.Value.UtcDateTime.ToEasternTimeZone();
			var startString = start.ToString("t");
			var end = session.EndTime.Value.UtcDateTime.ToEasternTimeZone();
			var endString = end.ToString("t");



			if (DateTime.Today.Year == start.Year)
			{
				if (DateTime.Today.DayOfYear == start.DayOfYear)
					return $"Today {startString}–{endString}";

				if (DateTime.Today.DayOfYear + 1 == start.DayOfYear)
					return $"Tomorrow {startString}–{endString}";
			}
			var day = start.ToString("M");
			return $"{day}, {startString}–{endString}";
		}


		public static string GetDisplayTime(this Session session)
		{
			if (!session.StartTime.HasValue || !session.EndTime.HasValue || session.StartTime.Value.UtcDateTime.IsTBA())
				return "TBA";
			var start = session.StartTime.Value.UtcDateTime.ToEasternTimeZone();


			var startString = start.ToString("t");
			var end = session.EndTime.Value.UtcDateTime.ToEasternTimeZone();
			var endString = end.ToString("t");
			return $"{startString}–{endString}";
		}


		public static IEnumerable<Grouping<string, Session>> FilterAndGroupByDate(this IList<Session> sessions)
		{
			if (Settings.Current.FavoritesOnly)
			{
				sessions = sessions.Where(s => s.IsFavorite).ToList();
			}

			var tba = sessions.Where(s => !s.StartTime.HasValue || !s.EndTime.HasValue || s.StartTime.Value.UtcDateTime.IsTBA());


			var showPast = Settings.Current.ShowPastSessions;
			var showAllCategories = Settings.Current.ShowAllCategories;
			var filteredCategories = Settings.Current.FilteredCategories;
			var utc = DateTime.UtcNow;


			//is not tba
			//has not started or has started and hasn't ended or ended 20 minutes ago
			//filter then by category and filters
			var grouped = (from session in sessions
						   where session.StartTime.HasValue && session.EndTime.HasValue && !session.StartTime.Value.UtcDateTime.IsTBA() && (showPast || (utc <= session.StartTime.Value || utc <= session.EndTime.Value.AddMinutes(20)))
						   && (showAllCategories || filteredCategories.IndexOf(session?.MainCategory?.Name ?? string.Empty, StringComparison.OrdinalIgnoreCase) >= 0)
						   orderby session.StartTimeOrderBy, session.Title
						   group session by session.GetSortName()
							into sessionGroup
						   select new Grouping<string, Session>(sessionGroup.Key, sessionGroup)).ToList();

			if (tba.Any())
				grouped.Add(new Grouping<string, Session>("TBA", tba));

			return grouped;
		}

		public static IEnumerable<Session> Search(this IEnumerable<Session> sessions, string searchText)
		{
			if (string.IsNullOrWhiteSpace(searchText))
				return sessions;

			var searchSplit = searchText.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			//search title, then category, then speaker name
			return sessions.Where(session =>
								  searchSplit.Any(search =>
								session.Haystack.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
		}
	}
}

