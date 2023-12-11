<Query Kind="Statements" />

var lines = File.ReadAllLines("c:/dev/advent_of_code/2023/day10/input.txt");

(int row, int col) sPos = (-1, -1);

for (int row = 0; row < lines.Length; row++)
{
	var line = lines[row];
	
	sPos = (row, line.IndexOf('S'));
	
	if (sPos.col != -1) break;
}

var prevPos = sPos;
var currPos = GetNextPosFromS(sPos.row, sPos.col);

var startFound = false;
int count;

for (count = 1; !startFound; count++)
{
	var tempPos = currPos;

	var currPipe = lines[currPos.row][currPos.col];

	// above
	if (prevPos.row < currPos.row)
	{
		if (currPipe == 'J') currPos = (currPos.row, currPos.col - 1);
		else if (currPipe == 'L') currPos = (currPos.row, currPos.col + 1);
		else if (currPipe == '|') currPos = (currPos.row + 1, currPos.col);
	}
	// right
	else if (prevPos.col > currPos.col)
	{
		if (currPipe == 'F') currPos = (currPos.row + 1, currPos.col);
		else if (currPipe == 'L') currPos = (currPos.row - 1, currPos.col);
		else if (currPipe == '-') currPos = (currPos.row, currPos.col - 1);
	}
	// down
	else if (prevPos.row > currPos.row)
	{
		if (currPipe == 'F') currPos = (currPos.row, currPos.col + 1);
		else if (currPipe == '7') currPos = (currPos.row, currPos.col - 1);
		else if (currPipe == '|') currPos = (currPos.row - 1, currPos.col);
	}
	// left
	else if (prevPos.col < currPos.col)
	{
		if (currPipe == '7') currPos = (currPos.row + 1, currPos.col);
		else if (currPipe == 'J') currPos = (currPos.row - 1, currPos.col);
		else if (currPipe == '-') currPos = (currPos.row, currPos.col + 1);
	}
	
	prevPos = tempPos;

	startFound = currPos == sPos;
}

(count / 2).Dump();

(int row, int col) GetNextPosFromS(int row, int col)
{
	var above = new HashSet<char> { '|', '7', 'F' };
	var right = new HashSet<char> { '-', '7', 'J' };
	var down = new HashSet<char> { '|', 'J', 'L' };
	var left = new HashSet<char> { '-', 'L', 'F' };

	if (row > 0)
	{
		var pipe = lines[row - 1][col];
		if (above.Contains(pipe))
		{
			return (row - 1, col);
		}
	}

	if (col < lines.Length - 1)
	{
		var pipe = lines[row][col + 1];
		if (right.Contains(pipe))
		{
			return (row, col + 1);
		}
	}

	if (row < lines.Length - 1)
	{
		var pipe = lines[row + 1][col];
		if (down.Contains(pipe))
		{
			return (row + 1, col);
		}
	}

	if (col > 0)
	{
		var pipe = lines[row][col - 1];
		if (left.Contains(pipe))
		{
			return (row, col - 1);
		}
	}

	throw new Exception("We found ourselves in a hopeless place. ~ Rihanna");
}