<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day1/input.txt");

int sum = 0;

foreach (var line in lines)
{
    int calibrationVal;
    var regexMatches = Regex.Matches(line, @"(\d)");
    var concat = regexMatches.First().Groups[0].Value + regexMatches.Last().Groups[0].Value;
    calibrationVal = int.Parse(concat);
    //$"{line}: {calibrationVal}".Dump();
    sum += calibrationVal;
}

sum.Dump();