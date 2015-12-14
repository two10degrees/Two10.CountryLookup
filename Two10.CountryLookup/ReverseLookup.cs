using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Two10.CountryLookup
{
    public class ReverseLookup
    {

        public Region[] Regions { get; private set; }

        public ReverseLookup()
        {
            this.Regions = ParseInput(LoadFile()).ToArray();
        }

        static bool InPolygon(float[] point, float[][] polygon)
        {
            var nvert = polygon.Length;
            var c = false;
            var i = 0;
            var j = 0;
            for (i = 0, j = nvert - 1; i < nvert; j = i++)
            {
                if (((polygon[i][1] > point[1]) != (polygon[j][1] > point[1])) && (point[0] < (polygon[j][0] - polygon[i][0]) * (point[1] - polygon[i][1]) / (polygon[j][1] - polygon[i][1]) + polygon[i][0]))
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
            var coords = new float[] { lng, lat };
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

        static IEnumerable<Region> ParseInput(IEnumerable<string> geojson)
        {
            foreach (var line in geojson)
            {
                foreach (var polygon in GeoJsonParser.Convert(line))
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

        static IEnumerable<string> LoadFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First()))
            using (var reader = new StreamReader(stream))
            {
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }


    }
}
