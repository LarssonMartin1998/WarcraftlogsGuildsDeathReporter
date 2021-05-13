using System.Collections.Generic;
using AngleSharp.Html.Dom;

namespace WarcraftlogsGuildsDeathReporter
{
	public class FinderBase
	{
		protected List<string> allFoundUrls = new List<string>();

		public string[] getResults()
		{
			string[] returnArray = null;

			if (allFoundUrls != null)
			{
				returnArray = allFoundUrls.ToArray();
			}

			return returnArray;
		}
	}
}