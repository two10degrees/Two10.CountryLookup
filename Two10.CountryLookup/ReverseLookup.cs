using System.Collections.Generic;
using System.Linq;
using Two10.CountryLookup.Abstractions;
using Two10.CountryLookup.Domain;
using Two10.CountryLookup.Services;

namespace Two10.CountryLookup
{
    public class ReverseLookup : IReverseLookup
    {
        private readonly IFileLoader _fileLoader;
        private readonly IGeoJsonParser _geoJsonParser;

        public Region[] Regions { get; private set; }

        public ReverseLookup() : this(new FileLoader(), new Services.GeoJsonParser())
        {

        }

        public ReverseLookup(IFileLoader fileLoader, IGeoJsonParser geoJsonParser)
        {
            _fileLoader = fileLoader;
            _geoJsonParser = geoJsonParser;
            this.Regions = ParseInput(_fileLoader.LoadFile()).ToArray();
        }

        private bool InPolygon(float[] point, float[][] polygon)
        {
            var nvert = polygon.Length;
            var c = false;
            var i = 0;
            var j = 0;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (polygon[i][1] > point[1] != (polygon[j][1] > point[1]) &&
                    point[0] < (polygon[j][0] - polygon[i][0]) * (point[1] - polygon[i][1]) /
                    (polygon[j][1] - polygon[i][1]) + polygon[i][0])
                {
                    c = !c;
                }
            }

            return c;
        }

        /// <summary>
        /// Looks up regions which the provided lat/lng fall inside
        /// </summary>
        /// <param name="lat">latitude</param>
        /// <param name="lng">longitude</param>
        /// <param name="types">optional region types to search. Defaults to all (land and sea)</param>
        /// <returns></returns>
        public Region Lookup(float lat, float lng, params RegionType[] types)
        {
            var coords = new[] { lng, lat };
            var subset = this.Regions as IEnumerable<Region>;

            if (types.Any())
            {
                subset = subset.Where(x => types.Any(y => y == x.Type));
            }

            foreach (var country in subset)
            {
                if (InPolygon(coords, country.Polygon))
                {
                    return country;
                }
            }

            return null;
        }

        private IEnumerable<Region> ParseInput(IEnumerable<string> geojson)
        {
            foreach (var line in geojson)
            {
                foreach (var polygon in _geoJsonParser.Convert(line))
                {
                    yield return new Region
                    {
                        Name = polygon.Properties["name"],
                        Type = polygon.Properties["type"] == "country" ? RegionType.Country : RegionType.Ocean,
                        Code = polygon.Id,
                        Polygon = polygon.Geometry
                    };
                }
            }
        }
    }
}
