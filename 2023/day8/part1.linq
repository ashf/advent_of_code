<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day8/input.txt");

var map = new Dictionary<string, (string left, string right)>();
string directions = null;

foreach (var line in lines)
{
    if (directions is null)
    {
        directions = line;
        continue;
    }
    
    if (string.IsNullOrWhiteSpace(line)) continue;
    
    var key = line.Split('=')[0].Trim();
    var left = line.Split('=')[1].Split(',')[0].Trim().TrimStart('(');
    var right = line.Split('=')[1].Split(',')[1].Trim().TrimEnd(')');
    
    map.Add(key, (left, right));
}

var node = "AAA";
var numSteps = 0;
var currentDirectionIndex = 0;
while (node != "ZZZ")
{
    numSteps++;
    currentDirectionIndex = (currentDirectionIndex == directions.Length - 1) ? 0 : currentDirectionIndex + 1;
    node = directions[currentDirectionIndex] == 'R' ? map[node].right : map[node].left;
}

numSteps.Dump();