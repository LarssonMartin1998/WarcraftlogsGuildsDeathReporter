#define HTML_PARSING_LOGGING
//#define HTML_ERROR_LOGGING

using System;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace WarcraftlogsGuildsDeathReporter
{
	public static class WebScraper
	{
		private static HttpClient httpClient = new HttpClient();
		private static HtmlParser parser = new HtmlParser();

		private static bool isDirty = true;

		private static void init()
		{
			isDirty = false;
#if DEBUG
#if HTML_PARSING_LOGGING
			parser.Parsing += onStartedParsing;
			parser.Parsed += onFinishedParsing;
#endif
#if HTML_ERROR_LOGGING
			parser.Error += onHtmlParserError;
#endif
#endif
		}

		public static async Task<IHtmlDocument> scrapeWebsite(string siteUrl)
		{
			if (isDirty)
			{
				init();
			}

			CancellationTokenSource cancellationToken = new CancellationTokenSource();
			HttpResponseMessage request = await httpClient.GetAsync(siteUrl);
			cancellationToken.Token.ThrowIfCancellationRequested();

			Stream response = await request.Content.ReadAsStreamAsync();
			cancellationToken.Token.ThrowIfCancellationRequested();

			return parser.ParseDocument(response);
		}

#if DEBUG
#if HTML_PARSING_LOGGING
		private static void onStartedParsing(object sender, AngleSharp.Dom.Events.Event ev)
		{
			Console.WriteLine("Started parsing html: " + ev.Time.ToLongTimeString());
		}

		private static void onFinishedParsing(object sender, AngleSharp.Dom.Events.Event ev)
		{
			Console.WriteLine("Finished parsing html: " + ev.Time.ToLongTimeString());
		}
#endif

#if HTML_ERROR_LOGGING
		private static void onHtmlParserError(object sender, AngleSharp.Dom.Events.Event ev)
		{
			if (ev is AngleSharp.Html.Dom.Events.HtmlErrorEvent)
			{
				AngleSharp.Html.Dom.Events.HtmlErrorEvent htmlErrorEvent = (AngleSharp.Html.Dom.Events.HtmlErrorEvent)ev;

				Console.WriteLine("HtmlParser encountered an error: " + htmlErrorEvent.Code.ToString() + " - " + htmlErrorEvent.Message);
			}
		}
#endif
#endif
	}
}