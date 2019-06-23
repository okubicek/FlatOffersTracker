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

				var elements = browser.FindElements(By.CssSelector(".property.ng-scope"));
				advertisements = elements.Select(el => new Advertisement
				{
					Url = el.FindElement(By.CssSelector(".title")).GetAttribute("href"),
					FlatType = query.FlatType,
					NumberOfRooms = query.NumberOfRooms,
					Address = el.FindElement(By.CssSelector(".locality")).GetAttribute("innerHTML"),
					Price = decimal.Parse(string.Join(string.Empty, el.FindElement(By.CssSelector(".norm-price")).GetAttribute("innerHTML").Where(x => Char.IsNumber(x)))),
				});
			}

			return advertisements;
		}
	}
}
