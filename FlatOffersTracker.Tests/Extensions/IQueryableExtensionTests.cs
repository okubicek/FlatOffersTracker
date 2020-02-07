using Common.ValueTypes;
using EFRepository.DataAccess.Extensions;
using FlatOffersTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FlatOffersTracker.Tests.Extensions
{
	public class IQueryableExtensionTests
	{
		public IQueryable<FlatOffer> _query;

		public IQueryableExtensionTests()
		{
			var offer1 = new FlatOffer
			{
				DateAdded = new DateTime(2020, 1, 10)
			};

			var offer2 = new FlatOffer
			{
				DateAdded = new DateTime(2019, 1, 10),
				DateRemoved = new DateTime(2020, 1, 10)
			};

			_query = new List<FlatOffer> { offer1, offer2 }.AsQueryable();
		}

		[Fact]
		public void DateWithinRangeReturnsExpectedValuesForNullable()
		{
			_query = _query.WhereDateWithinRange(x => x.DateRemoved, new DateRange(new DateTime(2020, 1, 1), null));
			var res = _query.ToList();
			Assert.Single(res);
		}

		[Fact]
		public void DateWithinRangeReturnsExpectedValues()
		{
			_query = _query.WhereDateWithinRange(x => x.DateAdded, new DateRange(new DateTime(2020, 1, 1), null));
			var res = _query.ToList();
			Assert.Single(res);
		}
	}
}
