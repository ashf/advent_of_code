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
	sum += GetFirstNumber(history);
}

sum.Dump();

int GetFirstNumber(List<int> history)
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
	
	var nextFirstNumber = GetFirstNumber(newHistory);
	
	return (history.First() - nextFirstNumber);
}