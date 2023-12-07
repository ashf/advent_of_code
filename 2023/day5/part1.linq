<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day5/input.txt");

List<long> seeds = new();
MapInput? mapInput = null;
Dictionary<MapInput, List<Map>> maps = new();

foreach (var line in lines)
{
    if (seeds.Count == 0)
    {
        seeds = line.Split(' ').Where(x => long.TryParse(x, out _)).Select(x => long.Parse(x)).ToList();
        continue;
    }
    
    if (string.IsNullOrWhiteSpace(line)) continue;
    
    var prevMapInput = mapInput;
    mapInput = line switch
    {
        "seed-to-soil map:" => MapInput.Seed2Soil,
        "soil-to-fertilizer map:" => MapInput.Soil2Fertilizer,
        "fertilizer-to-water map:" => MapInput.Fertilizer2Water,
        "water-to-light map:" => MapInput.Water2Light,
        "light-to-temperature map:" => MapInput.Light2Temperature,
        "temperature-to-humidity map:" => MapInput.Temperature2Humidity,
        "humidity-to-location map:" => MapInput.Humidity2Location,
        _ => mapInput,
    };
    if (prevMapInput != mapInput) continue;

    if (!maps.ContainsKey(mapInput.Value))
    {
        maps.Add(mapInput.Value, new List<Map>());
    }
    
    maps[mapInput.Value].Add(new Map(line));
}

foreach (var map in maps)
{
    maps[map.Key] = map.Value.OrderByDescending(x => x.SourceRangeStart).ToList();
}

var minLocation = long.MaxValue;

foreach (var seed in seeds)
{
    var soil = Source2Destination(maps[MapInput.Seed2Soil], seed);
    var fertilizer = Source2Destination(maps[MapInput.Soil2Fertilizer], soil);
    var water = Source2Destination(maps[MapInput.Fertilizer2Water], fertilizer);
    var light = Source2Destination(maps[MapInput.Water2Light], water);
    var temperature = Source2Destination(maps[MapInput.Light2Temperature], light);
    var humidity = Source2Destination(maps[MapInput.Temperature2Humidity], temperature);
    var location = Source2Destination(maps[MapInput.Humidity2Location], humidity);
    
    minLocation = Math.Min(minLocation, location);
}

minLocation.Dump();

long Source2Destination(List<Map> maps, long source)
{
    foreach (var map in maps)
    {
        if (source >= map.SourceRangeStart && source < map.SourceRangeStart + map.RangeLength)
        {
            return map.DestinationRangeStart + (source - map.SourceRangeStart);
        }
    }
    return source;
}

class Map
{
    public long DestinationRangeStart;
    public long SourceRangeStart;
    public long RangeLength;

    public Map(string line)
    {
        var split = line.Split(' ');
        DestinationRangeStart = long.Parse(split[0]);
        SourceRangeStart = long.Parse(split[1]);
        RangeLength = long.Parse(split[2]);
    }
}

enum MapInput
{
    Seed2Soil,
    Soil2Fertilizer,
    Fertilizer2Water,
    Water2Light,
    Light2Temperature,
    Temperature2Humidity,
    Humidity2Location
}
