using FluentAssertions;
using Two10.CountryLookup.Abstractions;
using Two10.CountryLookup.Services;
using Xunit;

namespace Two10.CountryLookup.Tests
{
    public class CountryLookUpIntegrationTests
    {
        private readonly IReverseLookup _reverseLookup;

        public CountryLookUpIntegrationTests()
        {
            IFileLoader fileLoader = new FileLoader();
            IGeoJsonParser geoJsonParser = new Services.GeoJsonParser();
            _reverseLookup = new ReverseLookup(fileLoader, geoJsonParser);
        }

        [Fact]
        public void TestDataLoad()
        {
            _reverseLookup.Regions.Length.Should().Be(394);

            foreach (var country in _reverseLookup.Regions)
            {
                country.Name.Should().NotBeNullOrEmpty();
                country.Code.Should().NotBeNullOrEmpty();
            }

            _reverseLookup.Regions.Should().Contain(x => x.Name == "United Kingdom");

            var ukResult = _reverseLookup.Lookup(52, 0);
            ukResult.Should().NotBeNull();
            ukResult.Name.Should().Be("United Kingdom");
        }

    }
}
