<Query Kind="Statements" />

var lines = File.ReadAllLines(@"c:\dev\advent_of_code\2023\day3\input.txt");

var sum = 0;

for (int row = 0; row < lines.Length; row++)
{
    var line = lines[row];
    
    var gearMatches = Regex.Matches(line, @"(\*)");
    
    foreach (Match gearMatch in gearMatches)
    {
        var index = gearMatch.Groups[0].Index;

        bool valid = false;
        
        List<int> matches = new();
        
        List<Match> numberMatches = Regex.Matches(lines[row], @"(\d+)").ToList();
        if (row > 0)
        {
            numberMatches.AddRange(Regex.Matches(lines[row - 1], @"(\d+)"));
        }
        
        if (row < lines.Length - 1)
        {
            numberMatches.AddRange(Regex.Matches(lines[row + 1], @"(\d+)"));
        }
        
        foreach (var numberMatch in numberMatches)
        {
            var number = numberMatch.Groups[0].Value;
            var startIndex = numberMatch.Groups[0].Index;
            var endIndex = startIndex + number.Length - 1;
            
            if (((index - 1) == endIndex) ||
                ((index + 1) == startIndex) ||
                (index >= startIndex && index <= endIndex))
            {
                matches.Add(int.Parse(number));     
            }
        }
        
        if (matches.Count == 2)
        {
            sum += (matches[0] * matches[1]);
        }
    }
}

sum.Dump();