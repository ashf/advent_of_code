<Query Kind="Statements">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day8/input.txt");

var map = new Dictionary<string, (string left, string right)>();
string directions = null;

var nodes = new List<string>();

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

    if (key.EndsWith('A'))
    {
        nodes.Add(key);
    }
}

long numSteps = 1;
foreach (var node in nodes)
{
    numSteps = LCM(numSteps, CountSteps(node));
}

numSteps.Dump();

long CountSteps(string node)
{
    long numSteps = 0;
    var currentDirectionIndex = 0;
    while (!node.EndsWith('Z'))
    {
        numSteps++;
        currentDirectionIndex = (currentDirectionIndex == directions.Length - 1) ? 0 : currentDirectionIndex + 1;
        node = directions[currentDirectionIndex] == 'R' ? map[node].right : map[node].left;
    }
    return numSteps;
}

long GCF(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

long LCM(long a, long b)
{
    return (a / GCF(a, b)) * b;
}