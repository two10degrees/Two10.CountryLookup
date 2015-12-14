namespace Two10.CountryLookup
{

    public enum RegionType
    {
        Country,
        Ocean
    }


    public class Region
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public RegionType Type { get; set; }
        public float[][] Polygon { get; set; }
    }
}
