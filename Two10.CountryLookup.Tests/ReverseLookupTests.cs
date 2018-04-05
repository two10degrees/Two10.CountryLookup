using System.Collections.Generic;
using FizzWare.NBuilder;
using FluentAssertions;
using Moq;
using Two10.CountryLookup.Abstractions;
using Two10.CountryLookup.Domain;
using Xunit;

namespace Two10.CountryLookup.Tests
{
    public class ReverseLookupTests
    {
        private readonly Mock<IFileLoader> _fileLoaderMock;
        private readonly Mock<IGeoJsonParser> _geoJsonParserMock;

        public ReverseLookupTests()
        {
            _fileLoaderMock = new Mock<IFileLoader>();
            _geoJsonParserMock = new Mock<IGeoJsonParser>();
        }

        [Fact]
        public void Lookup_PassingLondonCoordinates_Should_Get_Uk_Country_Back()
        {
            // Arrange
            var countries = new List<string> { "uk" };
            var propsDictionary = new Dictionary<string, string>
            {
                ["name"] = "United Kingdom",
                ["type"] = "country"
            };

            var ukCountryCoordinates = new[]
            {
                new[] {(float) -13.798828125, (float) 49.61070993807422},
                new[]
                {
                    (float) 1.7578125,
                    (float) 49.61070993807422
                },
                new[]
                {
                    (float) 1.7578125,
                    (float) 59.22093407615045
                },
                new[]
                {
                    (float) -13.798828125,
                    (float) 59.22093407615045
                },
                new[]
                {
                    (float) -13.798828125,
                    (float) 49.61070993807422
                }
            };


            var mockParsedJson = Builder<GeoJsonParser.ParsedGeoJson>.CreateListOfSize(100).All()
                .With(x => x.Id = "uk")
                .With(x => x.Geometry = ukCountryCoordinates)
                .With(x => x.Properties = propsDictionary)
                .Build();

            _fileLoaderMock.Setup(x => x.LoadFile()).Returns(countries);
            _geoJsonParserMock.Setup(x => x.Convert(It.IsAny<string>())).Returns(mockParsedJson);

            var reverseLookup = new ReverseLookup(_fileLoaderMock.Object, _geoJsonParserMock.Object);

            // Act
            var region = reverseLookup.Lookup(
                (float) 52.05249047600099, (float) -0.3515625, RegionType.Country);

            // Assert
            _fileLoaderMock.Verify(x => x.LoadFile(), Times.Once);
            _geoJsonParserMock.Verify(x => x.Convert(It.IsAny<string>()), Times.Once);

            region.Name.Should().Be("United Kingdom");
            region.Code.Should().Be("uk");
            region.Type.Should().Be(RegionType.Country);
        }
    }
}
