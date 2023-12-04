<Query Kind="Statements" />

var lines = File.ReadAllLines(@"c:\dev\advent_of_code\2023\day3\input.txt");

var sum = 0;

for (int row = 0; row < lines.Length; row++)
{
	var line = lines[row];
	
	var regexMatches = Regex.Matches(line, @"(\d+)");
	
	foreach (Match match in regexMatches)
	{
		var number = match.Groups[0].Value;
		var startIndex = match.Groups[0].Index;
		var endIndex = startIndex + number.Length -1;

		bool valid = false;
		
		for (int col = startIndex; col <= endIndex; col++)
		{
			if (row > 0 && col > 0 && IsSymbol(lines[row - 1][col - 1]))
			{
				valid = true;
				break;
			}

			if (row > 0 && IsSymbol(lines[row - 1][col]))
			{
				valid = true;
				break;
			}

			if (row > 0 && col < line.Length - 1  && IsSymbol(lines[row - 1][col + 1]))
			{
				valid = true;
				break;
			}

			if (col > 0 && IsSymbol(lines[row][col - 1]))
			{
				valid = true;
				break;
			}

			if (col < line.Length - 1 && IsSymbol(lines[row][col + 1]))
			{
				valid = true;
				break;
			}
			
			if (row < lines.Length - 1 && col > 0 && IsSymbol(lines[row + 1][col - 1]))
			{
				valid = true;
				break;
			}

			if (row < lines.Length - 1 && IsSymbol(lines[row + 1][col]))
			{
				valid = true;
				break;
			}

			if (row < lines.Length - 1 && col < line.Length - 1 && IsSymbol(lines[row + 1][col + 1]))
			{
				valid = true;
				break;
			}
		}
		
		if (valid)
		{
			sum += int.Parse(number);
		}
	}
}

sum.Dump();

bool IsSymbol(char input)
{
	char[] notSymbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '.' };
	return !notSymbols.Contains(input);
}