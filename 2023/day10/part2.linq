<Query Kind="Statements" />

var lines = File.ReadAllLines("c:/dev/advent_of_code/2023/day10/input4.txt");

(int row, int col) sPos = (-1, -1);

for (int row = 0; row < lines.Length; row++)
{
	var line = lines[row];
	
	sPos = (row, line.IndexOf('S'));
	
	if (sPos.col != -1) break;
}

var nextPositionsFromS = GetNextPositionsFromS(sPos.row, sPos.col);

lines = ReplaceSWithPipe(lines, sPos.row, sPos.col, nextPositionsFromS);

var loop = BuildLoop(lines, sPos, nextPositionsFromS.First());

var result = new int[lines.Length,lines[0].Length];\
var startPos = FirstOutsideEdge(lines, loop); // always 'F' 
var prevPos = startPos;
var currPos = (row: prevPos.row, col: prevPos.col + 1); // always go right from 'F'
var lastInside = (row: prevPos.row + 1, col: prevPos.col + 1);
result[lastInside.row, lastInside.col] = loop.Contains(lastInside) ? 'X' : 'I';

var loopFinished = false;
while (!loopFinished)
{
	var currPipe = lines[currPos.row][currPos.col];
	if (currPipe == '-')
	{
		var mod = lastInside.row > currPos.row ? 1 : -1;
		lastInside = (currPos.row + mod, currPos.col);
	}
	else if (currPipe == '|')
	{
		var mod = lastInside.col > currPos.col ? 1 : -1;
		lastInside = (currPos.row, currPos.col + mod);
	}
	else if (currPipe == 'F')
	{
		var mod = lastInside.col > currPos.col ? 1 : -1;
		lastInside = (currPos.row - 1, currPos.col + mod);
	}
	else if (currPipe == '7')
	{
		var mod = lastInside.col < currPos.col ? 1 : -1;
		lastInside = (currPos.row - 1, currPos.col + mod);
	}
	else if (currPipe == 'J')
	{
		var mod = lastInside.col < currPos.col ? 1 : -1;
		lastInside = (currPos.row + 1, currPos.col + mod);
	}
	else if (currPipe == 'L')
	{
		var mod = lastInside.col > currPos.col ? 1 : -1;
		lastInside = (currPos.row + 1, currPos.col + mod);
	}

	result[lastInside.row, lastInside.col] = loop.Contains(lastInside) ? 'X' : 'I';

	var tempPos = currPos;
	currPos = NextPos(currPos, prevPos);
	prevPos = tempPos;
	
	loopFinished = currPos == startPos;
}

result.Dump();

List<(int row, int col)> GetNextPositionsFromS(int row, int col)
{	
	var above = new HashSet<char> { '|', '7', 'F' };
	var right = new HashSet<char> { '-', '7', 'J' };
	var down = new HashSet<char> { '|', 'J', 'L' };
	var left = new HashSet<char> { '-', 'L', 'F' };

	var result = new List<(int row, int col)>();

	if (row > 0)
	{
		var pipe = lines[row - 1][col];
		if (above.Contains(pipe))
		{
			result.Add((row - 1, col));
		}
	}

	if (col < lines[0].Length - 1)
	{
		var pipe = lines[row][col + 1];
		if (right.Contains(pipe))
		{
			result.Add((row, col + 1));
		}
	}

	if (row < lines.Length - 1)
	{
		var pipe = lines[row + 1][col];
		if (down.Contains(pipe))
		{
			result.Add((row + 1, col));
		}
	}

	if (col > 0)
	{
		var pipe = lines[row][col - 1];
		if (left.Contains(pipe))
		{
			result.Add((row, col - 1));
		}
	}

	return result;
}

string[] ReplaceSWithPipe(string[] lines, int row, int col, List<(int row, int col)> nextPositionsFromS)
{
	var up = false;
	var right = false;
	var down = false;
	var left = false;

	foreach (var nextPosition in nextPositionsFromS)
	{
		if (nextPosition.row < row) { up = true; continue; }		
		if (nextPosition.col > col) { right = true; continue; }		
		if (nextPosition.row > row) { down = true; continue; }		
		if (nextPosition.col < col) { left = true; continue; }		
	}
	
	if (up && left) lines[row] = lines[row].Replace('S', 'J');
	if (up && right) lines[row] = lines[row].Replace('S', 'L');
	if (down && left) lines[row] = lines[row].Replace('S', '7');
	if (down && right) lines[row] = lines[row].Replace('S', 'F');
	if (left && right) lines[row] = lines[row].Replace('S', '-');
	if (up && down) lines[row] = lines[row].Replace('S', '|');

	return lines;
}

HashSet<(int row, int col)> BuildLoop(string[] lines, (int row, int col) sPos, (int row, int col) currPos)
{
	var loop = new HashSet<(int row, int col)>();

	var prevPos = sPos;

	var startFound = false;
	int count;

	for (count = 1; !startFound; count++)
	{
		var tempPos = currPos;

		currPos = NextPos(currPos, prevPos);

		prevPos = tempPos;

		startFound = currPos == sPos;

		loop.Add(currPos);
	}
	
	return loop;
}

(int row, int col) NextPos((int row, int col) currPos, (int row, int col) prevPos)
{
	var currPipe = lines[currPos.row][currPos.col];

	// above
	if (prevPos.row < currPos.row)
	{
		if (currPipe == 'J') return (currPos.row, currPos.col - 1);
		else if (currPipe == 'L') return (currPos.row, currPos.col + 1);
		else if (currPipe == '|') return (currPos.row + 1, currPos.col);
	}
	// right
	else if (prevPos.col > currPos.col)
	{
		if (currPipe == 'F') return (currPos.row + 1, currPos.col);
		else if (currPipe == 'L') return (currPos.row - 1, currPos.col);
		else if (currPipe == '-') return (currPos.row, currPos.col - 1);
	}
	// down
	else if (prevPos.row > currPos.row)
	{
		if (currPipe == 'F') return (currPos.row, currPos.col + 1);
		else if (currPipe == '7') return (currPos.row, currPos.col - 1);
		else if (currPipe == '|') return (currPos.row - 1, currPos.col);
	}
	// left
	else if (prevPos.col < currPos.col)
	{
		if (currPipe == '7') return (currPos.row + 1, currPos.col);
		else if (currPipe == 'J') return (currPos.row - 1, currPos.col);
		else if (currPipe == '-') return (currPos.row, currPos.col + 1);
	}

	throw new Exception("We found ourselves in a more hopeless place. ~ Rihanna");
}

(int row, int col) FirstOutsideEdge(string[] lines, HashSet<(int row, int col)> loop)
{
	for (int row = 0; row < lines.Length; row++)
	{
		for (int col = 0; col < lines[0].Length; col++)
		{
			if (loop.Contains((row, col)))
			{
				return (row, col);
			}
		}
	}

	throw new Exception("We found ourselves in a hopeless place. ~ Rihanna");
}

enum Direction
{
	Above,
	Below,
	Right,
	Left
}
