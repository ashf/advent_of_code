<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day6/input.txt").ToList();

var times = Regex.Matches(lines[0], @"\d+").Select(x => int.Parse(x.Value)).ToList();
var records = Regex.Matches(lines[1], @"\d+").Select(x => int.Parse(x.Value)).ToList();

List<Race> races = new List<Race>();

for (int i = 0; i < times.Count; i++)
{
	races.Add(new Race
	{
		Time = times[i],
		Record = records[i]
	});
}

var total = 1;
foreach (var race in races)
{
	total *= race.OptimalOptions.Count();
}
total.Dump();

class Race
{
	public int Time;
	public int Record;
	
	public IEnumerable<int> OptimalOptions =>
		Enumerable
			.Range(0, Time + 1)
			.Select(x => x * (Time - x))
			.Where(x => x > Record);
}