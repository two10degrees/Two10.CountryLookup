using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Two10.CountryLookup.Abstractions;

namespace Two10.CountryLookup.Services
{
    public class FileLoader : IFileLoader
    {
        public IEnumerable<string> LoadFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First()))
            using (var reader = new StreamReader(stream ?? throw new InvalidOperationException()))
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
