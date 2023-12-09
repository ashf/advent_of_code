<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day9/input.txt");

var sum = 0;

var histories = lines.Select(line =>
	line.Split(' ')
		.Select(x => int.Parse(x))
		.ToList())
	.ToList();


foreach (var history in histories)
{
	sum += GetLastNumber(history);
}

sum.Dump();

int GetLastNumber(List<int> history)
{
	if (history.All(x => x == 0) || history.Count == 1)
	{
		return 0;
	}
	
	List<int> newHistory = new();
	for (int i = 0; i < history.Count - 1; i++)
	{
		newHistory.Add(history[i+1] - history[i]);
	}
	
	var nextLastNumber = GetLastNumber(newHistory);
	
	return (history.Last() + nextLastNumber);
}