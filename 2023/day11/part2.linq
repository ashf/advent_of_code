<Query Kind="Statements" />

var lines = File.ReadAllLines("c:/dev/advent_of_code/2023/day11/input.txt");

(List<int> rowsToExpand, List<int> colsToExpand) = Expanded(lines);
var starLocations = FindStars(lines);
ShortestDistancesSummed(starLocations, rowsToExpand, colsToExpand).Dump();

(List<int> rows, List<int> cols) Expanded(string[] lines)
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
	
	return (rowsToExpand, colsToExpand);
}


List<(int row, int col)> FindStars(string[] expanded)
{
	List<(int row, int col)> starLocations = new();
	
	for (int row = 0; row < expanded.Length; row++)
	{
		for (int col = 0; col < expanded[0].Length; col++)
		{
			if (expanded[row][col] == '#') starLocations.Add((row, col));
		}
	}
	
	return starLocations;
}

long ShortestDistancesSummed(List<(int row, int col)> stars, List<int> rowsToExpand, List<int> colsToExpand)
{
	long shortestDistancesSummed = 0;
	
	for (int starIndex = 0; starIndex < stars.Count; starIndex++)
	{
		var star = stars[starIndex];
		
		for (int otherStarIndex = starIndex + 1; otherStarIndex < stars.Count; otherStarIndex++)
		{
			var otherStar = stars[otherStarIndex];
			
			var expandedRows = rowsToExpand.Where(row => row > Math.Min(star.row, otherStar.row) && row < Math.Max(star.row, otherStar.row)).Count() * (1_000_000 - 1);
			var expandedCols = colsToExpand.Where(col => col > Math.Min(star.col, otherStar.col) && col < Math.Max(star.col, otherStar.col)).Count() * (1_000_000 - 1);

			shortestDistancesSummed += Math.Abs(star.row - otherStar.row) + Math.Abs(star.col - otherStar.col) + expandedRows + expandedCols;
		}
	}
	
	return shortestDistancesSummed;
}