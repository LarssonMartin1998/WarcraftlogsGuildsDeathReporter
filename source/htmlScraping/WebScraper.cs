#define HTML_PARSING_LOGGING
//#define HTML_ERROR_LOGGING

using System;
using System.IO;
using System.Threading;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System.Threading;
using OpenQA.Selenium.Safari;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;

namespace WarcraftlogsGuildsDeathReporter
{
	public static class WebScraper
	{
		private static IWebDriver driver = null;

		private static HttpClient httpClient = new HttpClient();
		private static HtmlParser parser = new HtmlParser();

		public static void init()
		{
#if DEBUG
#if HTML_PARSING_LOGGING
			parser.Parsing += onStartedParsing;
			parser.Parsed += onFinishedParsing;
#endif
#if HTML_ERROR_LOGGING
			parser.Error += onHtmlParserError;
#endif
#endif

			startDriver();
		}

		public static void quit()
		{
			if (driver != null)
			{
				driver.Quit();
			}
		}

		private static void startDriver()
		{
			if (false)
			{
				InternetExplorerOptions ieOptions = new InternetExplorerOptions();

				ieOptions.PlatformName = "Windows 10";
				ieOptions.BrowserVersion = "11";
				//ieOptions.AddAdditionalCapability("username", username);
				//ieOptions.AddAdditionalCapability("password", authkey);
			}

			if (true)
			{
				if (driver == null)
				{
					driver = new FirefoxDriver();
				}

				//FirefoxOptions firefoxOptions = new FirefoxOptions();

				//firefoxOptions.PlatformName = "Windows 10";
				//firefoxOptions.BrowserVersion = "88";

				//var timeout = 10000;
				//driver = new FirefoxDriver("C:/Dev/Git/WarcraftlogsGuildsDeathReporter/thirdparty/", firefoxOptions, TimeSpan.FromMilliseconds(timeout));
			}

			if (false)
			{
				OperaOptions operaOptions = new OperaOptions();

				operaOptions.PlatformName = "Windows 10";
				operaOptions.BrowserVersion = "76";
				//operaOptions.AddAdditionalCapability("username", username, true);
				//operaOptions.AddAdditionalCapability("password", authkey, true);
			}
		}

		public static IWebDriver getDriver()
		{
			return driver;
		}

		public static void openNewTab()
		{
			IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
			js.ExecuteScript("window.open();");
		}

		public static async Task<IHtmlDocument> scrapeWebsite(string siteUrl)
		{
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