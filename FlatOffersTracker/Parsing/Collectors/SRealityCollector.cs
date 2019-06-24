using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FlatOffersTracker.Entities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace FlatOffersTracker.Parsing.Collectors
{
	public class SRealityCollector : IAdvertisementsCollector
	{
		private IQueryBuilder _queryBuilder = new SrealityQueryStringBuilder();

		private IEnumerable<Query> _queries = new List<Query> { };

		public IEnumerable<Advertisement> Collect()
		{
			throw new NotImplementedException();
		}

		private IEnumerable<Advertisement> CollectAdvertisements(Query query)
		{
			IEnumerable<Advertisement> advertisements;

			var url = _queryBuilder.GetQueryString(query);

			var chromeOptions = new ChromeOptions();
			chromeOptions.AddArguments("headless");
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			using (var browser = new ChromeDriver(path, chromeOptions))
			{
				browser.Navigate().GoToUrl(url);
				Console.WriteLine(browser.PageSource);

				SetMaxNumberOfAdvertisementsPerSinglePage(browser);

				var elements = browser.FindElements(By.CssSelector(".property.ng-scope"));

				advertisements = elements.Select(el => new Advertisement
				{
					Url = el.FindElement(By.CssSelector(".title")).GetAttribute("href"),
					FlatType = query.FlatType,
					NumberOfRooms = query.NumberOfRooms,
					Address = el.FindElement(By.CssSelector(".locality")).GetAttribute("innerHTML"),
					Price = ExtractPrice(el.FindElement(By.CssSelector(".norm-price")).GetAttribute("innerHTML")),
					FlatSize = ExtractFlatSize(elements[0].FindElement(By.CssSelector(".name")).GetAttribute("innerHTML"))
				});
			}

			return advertisements;
		}

		private static void SetMaxNumberOfAdvertisementsPerSinglePage(ChromeDriver browser)
		{
			var resultsPerPageButtons = browser.FindElement(By.CssSelector("options")).FindElements(By.TagName("button"));
			resultsPerPageButtons.Last().Click();
		}

		private int ExtractFlatSize(string name)
		{
			var parts = name.Split("&nbsp;");
			if (parts.Count() != 2)
			{
				return 0;
			}

			if (int.TryParse(parts[1], out var parsed))
			{
				return parsed;
			}

			return 0;
		}

		private decimal ExtractPrice(string price)
		{
			var extractedNumbers = string.Join(string.Empty, price.Where(x => Char.IsNumber(x)));

			return decimal.Parse(extractedNumbers);
		}
	}
}
