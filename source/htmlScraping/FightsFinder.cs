using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Net.Http;
using AngleSharp.Html.Parser;
using OpenQA.Selenium.Safari;
using System.Web;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;

namespace WarcraftlogsGuildsDeathReporter
{
	public class FightsFinder : FinderBase
	{
		public /*async Task */void findAllFights(string[] reportUrls)
		{
			IWebDriver driver = WebScraper.getDriver();
			
			// Make sure we have as many tabs opened as we have urls to cover.
			while (driver.WindowHandles.Count > 1)
			{
				driver.Close();
			}

			//for (int i = 0; i < reportUrls.Length - 1; ++i)
			//{
			//	WebScraper.openNewTab();
			//}

			//Task[] allTasks = new Task[reportUrls.Length];
			for (int i = 0; i < reportUrls.Length; ++i)
			{
				// TODO: Make this run in paralelle.
				/*allTasks[i] = Task.Run(() => */warcraftLogsReportScraping(driver, null/*driver.WindowHandles[i]*/, reportUrls[i])/*)*/;
			}

			//await Task.WhenAll(allTasks);
		}

        private void warcraftLogsReportScraping(IWebDriver driver, string windowHandle, string reportUrl)
        {
			//driver.SwitchTo().Window(windowHandle);
			driver.Navigate().GoToUrl(reportUrl);

            int timeout = 10000; /* Maximum wait time of 10 seconds */
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(timeout));
            wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            ReadOnlyCollection<IWebElement> allFightEntries = driver.FindElements(By.CssSelector(".wipes-entry"));

            foreach (IWebElement wipeTable in allFightEntries)
            {
				// Expected attribute value = "changeFightByIDAndIndex(fightId, unknownNumberParam, ususally 0, unknown bool param, unknown bool param)"
				string attributeVal = wipeTable.GetAttribute("onmousedown");
				if (!string.IsNullOrEmpty(attributeVal) && attributeVal.Contains("changeFightByIDAndIndex"))
				{
					// remove function name from the html code.
					string jsFuncParams = attributeVal.Split('(')[1];
					// Split the function params since were only interested about the 1st one which is the ID.
					string idString = jsFuncParams.Split(',')[0];
					int id = int.Parse(idString);

					allFoundUrls.Add(reportUrl + "#fight=" + idString);
				}
            }
        }

        public void clearData()
		{
			allFoundUrls.Clear();
		}
	}
}