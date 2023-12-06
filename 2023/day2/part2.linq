<Query Kind="Statements" />

using var file = File.Open(@"c:\dev\advent_of_code\2023\day2\input.txt", FileMode.Open);
using var streamReader = new StreamReader(file);

var sum = 0;

while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    int id = int.Parse(line.Split(' ')[1].Split(':')[0]);
    
    var game = line.Split(':')[1];
    var sets = game.Split(';');
    
    var minColors = new Dictionary<string,int?> { { "red",null }, { "green",null }, { "blue",null } };
    foreach (var set in sets)
    {
        var parts = set.Trim().Split(' ');
        for (int i = 0; i < parts.Length; i+=2)
        {
            int count = int.Parse(parts[i]);
            var color = parts[i+1].TrimEnd(',');
            if ((minColors[color] ?? 0) < count)
            {
                minColors[color] = count;
            }
        }
    }
    
    var power = (minColors["red"] ?? 1) * (minColors["green"] ?? 1) * (minColors["blue"] ?? 1);
    sum += power;
}

sum.Dump();