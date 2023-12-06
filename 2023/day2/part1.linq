<Query Kind="Statements" />

using var file = File.Open(@"c:\dev\advent_of_code\2023\day2\input.txt", FileMode.Open);
using var streamReader = new StreamReader(file);

var sum = 0;

var legal = new Dictionary<string, int> { { "red",12 }, { "green",13 }, { "blue",14} };

while (!streamReader.EndOfStream)
{
    var line = streamReader.ReadLine();
    int id = int.Parse(line.Split(' ')[1].Split(':')[0]);

    var game = line.Split(':')[1];
    var sets = game.Split(';');
    bool valid = true;
    foreach (var set in sets)
    {
        var parts = set.Trim().Split(' ');
        for (int i = 0; i < parts.Length; i+=2)
        {
            int count = int.Parse(parts[i]);
            var color = parts[i+1].TrimEnd(',');
            if (legal[color] < count)
            {
                $"INVALID. {color}: {count} > {legal[color]}... {line}".Dump();
                valid = false;
                break;
            }
        }
        if (!valid) break;
    }

    if (valid)
    {
        $"VALID. {line}".Dump();
        sum += id;
    }
}

sum.Dump();