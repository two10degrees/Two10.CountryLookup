using System.Collections.Generic;
using Two10.CountryLookup.Domain;

namespace Two10.CountryLookup.Abstractions
{
    public interface IGeoJsonParser
    {
        IEnumerable<GeoJsonParser.ParsedGeoJson> Convert(dynamic json);
        IEnumerable<GeoJsonParser.ParsedGeoJson> Convert(string json);
    }
}