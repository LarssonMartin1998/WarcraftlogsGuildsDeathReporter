using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;

namespace WarcraftlogsGuildsDeathReporter
{
	public class FightsFinder : FinderBase
	{
		public async Task findAllFights(string reportUrl)
		{
			clearDirtyData();

			IHtmlDocument htmlDoc = await WebScraper.scrapeWebsite(reportUrl);

			foreach (IElement element in htmlDoc.All)
			{
				if (!string.IsNullOrEmpty(element.Id) && element.Id.Contains("report-overview-fights-contents"))
				{
					// AngleSharp cant find the HTML for the contents inside of # report-overview-fights-contents.
					// The reason being is because its not part of the source HTML, its probably generated in runtime from a database.

					// TODO: Find a solution to scrape dynamically generated HTML.
					// Note - Look into Puppeteer.
				}
			}

			//await findAdditionalReportPages(rootDoc);
		}

		private void clearDirtyData()
		{
			allFoundUrls.Clear();
		}
	}
}