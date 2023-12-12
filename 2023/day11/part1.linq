<Query Kind="Statements" />

var lines = File.ReadAllLines("c:/dev/advent_of_code/2023/day11/input.txt");

var expanded = Expand(lines);
var starLocations = FindStars(expanded);
ShortestDistancesSummed(starLocations).Dump();

List<string> Expand(string[] lines)
{
	List<int> colsToExpand = new();
	List<int> rowsToExpand = new();
	var expanded = new List<string>();
	
	for (int row = 0; row < lines.Length; row++)
	{
		var line = lines[row];
		expanded.Add(line);
		
		if (line.All(c => c == '.')) rowsToExpand.Add(row);
	}
	
	for (int col = 0; col < lines[0].Length; col++)
	{
		var expandCol = true;
		for (int row = 0; row < lines.Length && expandCol; row++)
		{
			if (lines[row][col] != '.')
			{
				expandCol = false;
			}
		}
		
		if (expandCol) colsToExpand.Add(col);
	}


	int rowsExpanded = 0;
	foreach (var row in rowsToExpand)
	{
		expanded.Insert(row + rowsExpanded, new string('.', lines[0].Length));
		rowsExpanded++;
	}
	
	int colsExpanded = 0;
	foreach (var col in colsToExpand)
	{
		for (int row = 0; row < expanded.Count; row++)
		{
			expanded[row] = expanded[row].Insert(col + colsExpanded, ".");
		}
		colsExpanded++;
	}
	
	return expanded;
}


List<(int row, int col)> FindStars(List<string> expanded)
{
	List<(int row, int col)> starLocations = new();
	
	for (int row = 0; row < expanded.Count; row++)
	{
		for (int col = 0; col < expanded[0].Length; col++)
		{
			if (expanded[row][col] == '#') starLocations.Add((row, col));
		}
	}
	
	return starLocations;
}

int ShortestDistancesSummed(List<(int row, int col)> stars)
{
	int shortestDistancesSummed = 0;
	
	for (int starIndex = 0; starIndex < stars.Count; starIndex++)
	{
		var star = stars[starIndex];
		
		for (int otherStarIndex = starIndex + 1; otherStarIndex < stars.Count; otherStarIndex++)
		{			
			var otherStar = stars[otherStarIndex];
			
			shortestDistancesSummed += Math.Abs(star.row - otherStar.row) + Math.Abs(star.col - otherStar.col);
		}
	}
	
	return shortestDistancesSummed;
}