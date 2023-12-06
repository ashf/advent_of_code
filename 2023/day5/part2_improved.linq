<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
  <Namespace>System.Collections.Concurrent</Namespace>
</Query>

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day5/input.txt").ToList();

MapInput? mapInput = null;
Dictionary<MapInput, List<Map>> maps = new();

string seedLine = string.Empty;

foreach (var line in lines)
{
    if (seedLine == string.Empty)
    {
        seedLine = line;        
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

var minLocation = long.MaxValue;
var lockObj = new object();

var seedRangeMatches = Regex.Matches(seedLine, @"(\d+) (\d+)");
Parallel.ForEach(seedRangeMatches, seedRangeMatch =>
{
    var start = long.Parse(seedRangeMatch.Groups[1].Value);
    var length = long.Parse(seedRangeMatch.Groups[2].Value);

    Queue<Range> ranges = new Queue<Range>();
    ranges.Enqueue(new Range { Start = start, End = start + length});
    
    ranges = GetMappedRanges(maps[MapInput.Seed2Soil], ranges);
    ranges = GetMappedRanges(maps[MapInput.Soil2Fertilizer], ranges);
    ranges = GetMappedRanges(maps[MapInput.Fertilizer2Water], ranges);
    ranges = GetMappedRanges(maps[MapInput.Water2Light], ranges);
    ranges = GetMappedRanges(maps[MapInput.Light2Temperature], ranges);
    ranges = GetMappedRanges(maps[MapInput.Temperature2Humidity], ranges);
    ranges = GetMappedRanges(maps[MapInput.Humidity2Location], ranges);

    lock (lockObj)
    {
        minLocation = Math.Min(minLocation, ranges.Min(x => x.Start));
    }
});

minLocation.Dump();

Queue<Range> GetMappedRanges(List<Map> maps, Queue<Range> ranges)
{
    Queue<Range> result = new();
    while (ranges.Count > 0)
    {
        var range = ranges.Dequeue();
        var totalMiss = true;
        foreach (var map in maps)
        {
            (var match, var misses) = map.DestinationIntersection(range);
            if (match.HasValue)
            {
                result.Enqueue(match.Value);
                totalMiss = false;
            }
            foreach (var miss in misses)
            {
                ranges.Enqueue(miss);               
            }
        }
        if (totalMiss)
        {
            result.Enqueue(range);
        }
    }
    
    return result;
}

struct Range
{
    public long Start;
    public long End;
    
    public bool Intersect(Range other)
    {
        return (other.End >= this.Start) && (other.Start <= this.End);
    }
}

struct Map
{
    public Range SourceRange;
    public long DestinationRangeStart;

    public Map(string line)
    {
        var split = line.Split(' ');
        DestinationRangeStart = long.Parse(split[0]);
        var sourceRangeStart = long.Parse(split[1]);
        var rangeLength = long.Parse(split[2]);
        SourceRange = new Range { Start = sourceRangeStart, End = sourceRangeStart + rangeLength - 1 };
    }

    public (Range? match, List<Range> unmatched) DestinationIntersection(Range range)
    {
        Range? match = null;
        List<Range> unmatched = new();
        
        if (SourceRange.Intersect(range))
        {
            var start = Math.Max(range.Start, SourceRange.Start);
            var end = Math.Min(range.End, SourceRange.End);
            var startDiff = start - SourceRange.Start;
            var length = end - start;
            match = new Range { Start = DestinationRangeStart + startDiff, End = DestinationRangeStart + startDiff + length};
            
            long? preStart = range.Start < start ? range.Start : null;
            long? preEnd = preStart.HasValue ? start-1 : null;

            long? postEnd = range.End > end ? range.End : null;
            long? postStart = postEnd.HasValue ? end+1 : null;
            
            if (preStart.HasValue)
            {
                unmatched.Add(new Range { Start = preStart.Value, End = preEnd.Value });
            }
            if (postStart.HasValue)
            {
                unmatched.Add(new Range { Start = postStart.Value, End = postEnd.Value });
            }
        }

        return (match, unmatched);
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
