<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day6/input.txt").ToList();

var race = new Race
{
	Time = long.Parse(lines[0].Replace(" ", "").Split(':')[1]),
	Record = long.Parse(lines[1].Replace(" ", "").Split(':')[1])
};

race.OptimalOptions().Count().Dump();

class Race
{
	public long Time;
	public long Record;
	
	public IEnumerable<long> OptimalOptions()
	{
		List<long> options = new();
		for (long i = 0; i <= Time; i++)
		{
			var traveled = i * (Time - i);
			if (traveled > Record)
			{
				options.Add(traveled);
			}
		}
		return options;
	}
}