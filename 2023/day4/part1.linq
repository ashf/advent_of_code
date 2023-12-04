<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day4/input.txt");

var sum = 0;

foreach (var line in lines)
{
	var firstSplit = line.Split('|');
	var winners = firstSplit[0].Split(':')[1].Trim().Split(' ').Where(x => x != "").Select(x => int.Parse(x));
	var guesses = firstSplit[1].Trim().Split(' ').Where(x => x != "").Select(x => int.Parse(x));
	
	var matchCount = winners.Intersect(guesses).Count();
	sum += (int)Math.Pow(2, matchCount - 1);
}

sum.Dump();