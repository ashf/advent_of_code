<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day6/input.txt").ToList();

var race = new Race
{
	Time = long.Parse(lines[0].Replace(" ", "").Split(':')[1]),
	Record = long.Parse(lines[1].Replace(" ", "").Split(':')[1])
};

race.OptimalOptions_Naive().Dump();
race.OptimalOptionsCount_Optimal().Dump();

class Race
{
	public long Time;
	public long Record;
	
	public int OptimalOptions_Naive()
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
		return options.Count();
	}
	
	public int OptimalOptionsCount_Optimal()
	{
		var upperBoundary = Math.Floor((Time + Math.Sqrt(Math.Pow(Time, 2) - 4 * Record)) / 2);

		var lowerBoundary = Math.Ceiling((Time - Math.Sqrt(Math.Pow(Time, 2) - 4 * Record)) / 2);

		return (int)(upperBoundary - lowerBoundary + 1);
	}
}