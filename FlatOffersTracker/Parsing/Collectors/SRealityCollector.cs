using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FlatOffersTracker.Entities;
using FlatOffersTracker.Parsing.QuerySets;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Serilog;

namespace FlatOffersTracker.Parsing.Collectors
{
	public class SRealityCollector : IAdvertisementsCollector
	{
		private ILogger _logger;

		private IQueryBuilder _queryBuilder = new SrealityQueryStringBuilder();

		private IEnumerable<Query> _queries = new List<Query> {
			new Brick2RoomsQuery(),
			new Brick3RoomsQuery(),
			new Panel3RoomsQuery()
		};

		public SRealityCollector(ILogger logger)
		{
			_logger = logger;
		}

		public IEnumerable<Advertisement> Collect()
		{
			var advertisements = new List<Advertisement>();

			foreach(var query in _queries)
			{
				var collected = CollectAdvertisements(query);
				advertisements.AddRange(collected);
			}

			return advertisements;
		}

		private IEnumerable<Advertisement> CollectAdvertisements(Query query)
		{
			var url = _queryBuilder.GetQueryString(query);

			using (var browser = SetupDriver())
			{
				browser
					.Navigate()
					.GoToUrl(url);

				SetMaxNumberOfAdvertisementsPerSinglePage(browser);
				var elements = GetIndividualAdvertisements(browser);

				var advertisements = elements.Select(el => new Advertisement
				{
					Url = el.FindElement(By.CssSelector(".title")).GetAttribute("href"),
					FlatType = query.FlatType,
					NumberOfRooms = query.NumberOfRooms,
					Address = el.FindElement(By.CssSelector(".locality")).GetAttribute("innerHTML"),
					Price = ExtractPrice(el.FindElement(By.CssSelector(".norm-price")).GetAttribute("innerHTML")),
					FlatSize = ExtractFlatSize(el.FindElement(By.CssSelector(".name")).GetAttribute("innerHTML")),
					UniqueId = ExtractUid(el.FindElement(By.CssSelector(".title")).GetAttribute("href"))
				}).ToList();

				_logger.Information("url {url} pulled {count} advertisements", url, advertisements.Count);

				return advertisements;
			}
		}

		private static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetIndividualAdvertisements(ChromeDriver browser)
		{
			var wait = new DefaultWait<IWebDriver>(browser);
			wait.Timeout = TimeSpan.FromMilliseconds(10000);
			wait.PollingInterval = TimeSpan.FromMilliseconds(200);
			wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
			wait.Until(x => { return x.FindElement(By.CssSelector(".property.ng-scope")); });

			return browser.FindElements(By.CssSelector(".property.ng-scope"));
		}

		private ChromeDriver SetupDriver()
		{
			var chromeOptions = new ChromeOptions();
			chromeOptions.AddArguments("headless");
			var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

			return new ChromeDriver(path, chromeOptions);
		}

		private static void SetMaxNumberOfAdvertisementsPerSinglePage(ChromeDriver browser)
		{
			browser
				.FindElement(By.CssSelector(".per-page"))
				.FindElement(By.CssSelector(".selected"))
				.Click();
			browser
				.FindElement(By.CssSelector(".per-page"))
				.FindElement(By.CssSelector(".options"))
				.FindElements(By.TagName("button"))
				.Last()
				.Click();
		}

		private int ExtractFlatSize(string name)
		{
			var parts = name.Split(new string[] { "&nbsp;", " " }, StringSplitOptions.RemoveEmptyEntries);

			var numberOfparts = parts.Count();

			if (parts.Count() < 2)
			{
				return 0;
			}

			if (int.TryParse(parts[numberOfparts - 2], out var parsed))
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

		private int ExtractUid(string url)
		{
			var i = url.LastIndexOf('/');
			var stringId = url.Substring(i + 1, (url.Length - i) - 1);

			if (int.TryParse(stringId, out var id))
			{
				return id;
			}

			return HashCode.Combine(url);
		}
	}
}