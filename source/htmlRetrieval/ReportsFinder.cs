using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;

namespace WarcraftlogsGuildsDeathReporter
{
	public class ReportsFinder
	{
		List<IHtmlDocument> allPagesHtmlDocs = new List<IHtmlDocument>();
		List<string> extraReportPagesUrls = new List<string>();
		List<string> allReportUrls = new List<string>();

		public async Task findAllReports(string guildReportsUrl)
		{
			clearDirtyData();

			IHtmlDocument rootDoc = await WebScraper.scrapeWebsite(guildReportsUrl);
			allPagesHtmlDocs.Add(rootDoc);

			await findAdditionalReportPages(rootDoc);
			findAllReportUrls();
		}

		private void clearDirtyData()
		{
			allPagesHtmlDocs.Clear();
			extraReportPagesUrls.Clear();
			allReportUrls.Clear();
		}

		private async Task findAdditionalReportPages(IHtmlDocument rootDoc)
		{
			extraReportPagesUrls.Clear();

			foreach (IElement element in rootDoc.All)
			{
				string hrefVal = element.GetAttribute("href");
				string relVal =	element.GetAttribute("rel");
				if (!string.IsNullOrEmpty(hrefVal) && // the current page has an empty href attribute, ignore it since we already have it.
					string.IsNullOrEmpty(relVal) && // previois and next arrows have rel attribues, while the page links dont.
					element.ChildElementCount == 0 &&
					element.ClassName == "page-link" &&
					hrefVal.Contains(CommonConstants.kGuildReportsListUrl))
				{
					extraReportPagesUrls.Add(hrefVal);
				}
			}

			if (extraReportPagesUrls.Count > 0)
			{
				foreach (string url in extraReportPagesUrls)
				{
					allPagesHtmlDocs.Add(await WebScraper.scrapeWebsite(url));
				}
			}
		}

		private void findAllReportUrls()
		{
			foreach (IHtmlDocument document in allPagesHtmlDocs)
			{
				foreach (IElement element in document.All)
				{
					if (element.ChildElementCount == 0 && element.OuterHtml.Contains("<a href=\"/reports/"))
					{
						allReportUrls.Add(CommonConstants.kWarcraftlogsUrl + element.GetAttribute("href"));
					}
				}
			}
		}

		public string[] getResults()
		{
			string[] returnArray = null;

			if (allReportUrls != null)
			{
				returnArray = allReportUrls.ToArray();
			}

			return returnArray;
		}
	}
}