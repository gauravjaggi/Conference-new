using System;
using System.Collections.Generic;
using EventList.Models;

namespace EventList
{
	public interface IFileWriter
	{
		void CreateCsvFile(string csv, string filename);

		List<Models.Session>  GetSessionFromCsv();

		List<FeaturedEvent> GetEventsFromCsv();

		List<MiniHack> GetMiniHacksFromCsv();
	}
}
