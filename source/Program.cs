using System;
using System.Threading.Tasks;

namespace WarcraftlogsGuildsDeathReporter
{
	static class Program
	{
		static public void Main(String[] args)
		{
			MainAsync(args).GetAwaiter().GetResult();
			Console.ReadLine();
		}

		static private async Task MainAsync(string[] args)
		{
			ReportsFinder reportsFinder = new ReportsFinder();
			await reportsFinder.findAllReports("https://www.warcraftlogs.com/guild/reports-list/559073?zone=26&boss=0&difficulty=0&class=Any&spec=Any&keystone=0&kills=0&duration=0");
			string[] results = reportsFinder.getResults();

			FightsFinder fightsFinder = new FightsFinder();
			foreach (string reportUrl in results)
			{
				await fightsFinder.findAllFights(reportUrl);
			}

			Console.WriteLine("\n------------------------------------------------------------------------\n");
			for (int i = 0; i < results.Length; ++i)
			{
				Console.WriteLine((i + 1).ToString() + " - " + results[i]);
			}
		}
	}
}