using Two10.CountryLookup.Domain;

namespace Two10.CountryLookup.Abstractions
{
    public interface IReverseLookup
    {
        Region[] Regions { get; }

        Region Lookup(float lat, float lng, params RegionType[] types);
    }
}