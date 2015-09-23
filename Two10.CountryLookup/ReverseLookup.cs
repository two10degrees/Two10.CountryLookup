using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Two10.CountryLookup
{
    public class ReverseLookup
    {

        public Country[] Countries { get; private set; }

        public ReverseLookup()
        {
            this.Countries = ParseInput(LoadFile()).ToArray();
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

        public Country Lookup(float lat, float lng)
        {
            var coords = new float[] { lng, lat };
            foreach (var country in this.Countries)
            {
                if (InPolygon(coords, country.Polygon))
                {
                    return country;
                }
            }
            return null;
        }

        static IEnumerable<Country> ParseInput(IEnumerable<string> geojson)
        {
            foreach (var line in geojson)
            {
                foreach (var polygon in GeoJsonParser.Convert(line))
                {
                    yield return new Country
                    {
                        Name = polygon.Properties["name"],
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
