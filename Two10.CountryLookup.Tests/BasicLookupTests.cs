using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Two10.CountryLookup.Tests
{
    [TestClass]
    public class BasicLookupTests
    {
        [TestMethod]
        public void TestDataLoad()
        {
            var lookup = new ReverseLookup();
            Assert.AreNotEqual(0, lookup.Regions.Length);

            foreach (var country in lookup.Regions)
            {
                Assert.IsNotNull(country.Name, "country.name");
                Assert.IsNotNull(country.Code, "country.code");
            }

            Assert.AreNotEqual(0, lookup.Regions.Count(x => x.Name == "United Kingdom"));
            Assert.AreNotEqual(1, lookup.Regions.Count(x => x.Name == "United Kingdom"));

            var ukResult = lookup.Lookup(52, 0);
            Assert.IsNotNull(ukResult, "uk result");
            Assert.AreEqual("United Kingdom", ukResult.Name, "uk");


            Assert.IsNull(lookup.Lookup(0, 0, RegionType.Country), "ocean");

            var result = lookup.Lookup(0, 0, RegionType.Ocean);
            Assert.IsNotNull(result);
            Assert.AreEqual("North Atlantic Ocean", result.Name);
        }

    }
}
