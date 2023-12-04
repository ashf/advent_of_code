<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day1/input.txt");

int sum = 0;
var wordToNum = new Dictionary<string, int>
{
	{"one", 1},
	{"two", 2},
	{"three", 3},
	{"four", 4},
	{"five", 5},
	{"six", 6},
	{"seven", 7},
	{"eight", 8},
	{"nine", 9},
};

foreach (var line in lines)
{
	var cleanLine = line
		.Replace("two", "t2wo")
		.Replace("seven", "sev7en");
	
	int calibrationVal;

	var regexMatches = Regex.Matches(cleanLine, @"one|two|three|four|five|six|seven|eight|nine|\d");
	var first = regexMatches.First().Groups[0].Value;
	var last = regexMatches.Last().Groups[0].Value;
	
	var firstVal = int.TryParse(first, out var foo) ? foo : wordToNum[first];
	var lastVal = int.TryParse(last, out var bar) ? bar : wordToNum[last];
	var concat = $"{firstVal}{lastVal}";
	int.TryParse(concat, out calibrationVal);
	
	//$"{line}: {calibrationVal}".Dump();
	sum += calibrationVal;
}

sum.Dump();
