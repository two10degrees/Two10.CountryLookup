using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Two10.CountryLookup.Abstractions;

namespace Two10.CountryLookup.Services
{
    public partial class GeoJsonParser : IGeoJsonParser
    {

        public IEnumerable<Domain.GeoJsonParser.ParsedGeoJson> Convert(string json)
        {
            return Convert(JsonConvert.DeserializeObject<dynamic>(json));
        }

        public IEnumerable<Domain.GeoJsonParser.ParsedGeoJson> Convert(dynamic json)
        {
            foreach (var coords in ToPoints(json.geometry))
            {
                yield return new Domain.GeoJsonParser.ParsedGeoJson
                {
                    Id = json.id,
                    Geometry = coords,
                    Properties =  ((IEnumerable<KeyValuePair<string, JToken>>)json.properties)
                                        .ToDictionary(kvp => kvp.Key.ToLower(), kvp => kvp.Value.ToString())
                };
            }
        }


        private IEnumerable<float[][]> ToPoints(dynamic geometry)
        {
            if (null == geometry) yield break;
            switch ((string)geometry.type)
            {
                case "Polygon":
                    yield return (ParseCoordinates((dynamic) geometry.coordinates[0]) as IEnumerable<float[]>).ToArray();
                    break;
                case "MultiPolygon":
                    foreach (dynamic item in geometry.coordinates)
                    {
                        yield return (ParseCoordinates(item[0]) as IEnumerable<float[]>).ToArray();
                    }
                    break;
                default:
                    throw new NotImplementedException((string)geometry.type);
            }
        }

        private IEnumerable<float[]> ParseCoordinates(dynamic coords)
        {
            foreach (var coord in coords)
            {
                yield return new float[] { (float)coord[0], (float)coord[1] };
            }
        }
       
    }
}
