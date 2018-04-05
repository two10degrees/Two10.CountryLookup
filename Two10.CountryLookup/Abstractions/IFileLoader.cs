using System.Collections.Generic;

namespace Two10.CountryLookup.Abstractions
{
    public interface IFileLoader
    {
        IEnumerable<string> LoadFile();
    }
}