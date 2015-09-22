# Two10.CountryLookup

## Installation

```
> Install-Package Two10.CountryLookup
```

## Usage

```c#
var lookup = new ReverseLookup();

// look up a country by lat / lng
var country = lookup(52.0, 2.0);

Console.WriteLine(country.Name); // "United Kingdon"
```

Creating a ReverseLookup object is expensive, so it's worth keeping it as a singleton.

## License

MIT