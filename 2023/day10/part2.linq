<Query Kind="Statements" />

var lines = File.ReadAllLines("c:/dev/advent_of_code/2023/day10/input.txt");

(int row, int col) sPos = (-1, -1);

for (int row = 0; row < lines.Length; row++)
{
	var line = lines[row];
	
	sPos = (row, line.IndexOf('S'));
	
	if (sPos.col != -1) break;
}

var nextPositionsFromS = GetNextPositionsFromS(sPos.row, sPos.col);

// replace the starting position with appropriate pipe piece
lines = ReplaceSWithPipe(lines, sPos.row, sPos.col, nextPositionsFromS);

var result = new char[lines.Length, lines[0].Length];
result[sPos.row, sPos.col] = 'X';

var loop = BuildLoop(lines, sPos, nextPositionsFromS.First(), result);
result.Dump();

var iCount = 0;

// if any 'I' is on the outside we need to flip 'I's to 'O's
// mark any outside default to 'O'
bool flip = false;
for (int row = 0; row < lines.Length; row++)
{
    for (int col = 0; col < lines[0].Length; col++)
    {
        if (row == 0 && result[row, col] == 'I') flip = true;
        if (row == lines.Length - 1 && result[row, col] == 'I') flip = true;
        if (col == 0 && result[row, col] == 'I') flip = true;
        if (col == lines[0].Length - 1 && result[row, col] == 'I') flip = true;

        if (row == 0 && result[row, col] == default) result[row, col] = 'O';
        if (row == lines.Length - 1 && result[row, col] == default) result[row, col] = 'O';
        if (col == 0 && result[row, col] == default) result[row, col] = 'O';
        if (col == lines[0].Length - 1 && result[row, col] == default) result[row, col] = 'O';
    }
}
result.Dump();

// flip 'I's to 'O's
if (flip)
{
    for (int row = 0; row < lines.Length; row++)
    {
        for (int col = 0; col < lines[0].Length; col++)
        {
            if (result[row, col] == 'I') result[row, col] = 'O';
        }
    }
}

result.Dump();

for (int row = 0; row < lines.Length; row++)
{
    for (int col = 0; col < lines[0].Length; col++)
    {
        // if not determined, then determine if 'I' or 'O' (flip if needed)
        var marked = new bool[lines.Length, lines[0].Length];
        if (result[row, col] == default)
        {
            result[row, col] = flip && IsInside(row, col, result, marked) ? 'I' : 'O';
        }
        
        if (result[row, col] == 'I') iCount++;
    }
}

result.Dump();

iCount.Dump();

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

HashSet<(int row, int col)> BuildLoop(string[] lines, (int row, int col) sPos, (int row, int col) currPos, char[,] result)
{
	var loop = new HashSet<(int row, int col)>();

	var prevPos = sPos;

	var startFound = false;
	int count;

	for (count = 1; !startFound; count++)
	{
		var tempPos = currPos;

		currPos = NextPos(currPos, prevPos, result);

		prevPos = tempPos;

		startFound = currPos == sPos;

		loop.Add(currPos);
	}
	
	return loop;
}

// build loop and mark on results Insides (may need to be flipped later)
(int row, int col) NextPos((int row, int col) currPos, (int row, int col) prevPos, char[,] result)
{
	var currPipe = lines[currPos.row][currPos.col];

    var mods = new List<(int row, int col)> { (0, 0), (0, 0), (0, 0) };
    
    (int row, int col) newPos = (0, 0);

    // above
	if (prevPos.row < currPos.row)
    {
        if (currPipe == 'J')
        {
            mods[0] = (-1, -1);
            newPos = (currPos.row, currPos.col - 1);
        }
        else if (currPipe == 'L')
        {
            mods[0] = (-0, 1);
            mods[1] = (1, 0);
            mods[2] = (1, -1);
            newPos = (currPos.row, currPos.col + 1);
        }
        else if (currPipe == '|')
        {
            mods[0] = (-1, -1);
            mods[1] = (0, -1);
            mods[2] = (1, -1);
            newPos = (currPos.row + 1, currPos.col);
        }
	}
	// right
	else if (prevPos.col > currPos.col)
    {
        if (currPipe == 'F')
        {
            mods[0] = (-1, 0);
            mods[1] = (0, -1);
            mods[2] = (-1, -1);
            newPos = (currPos.row + 1, currPos.col);
        }
        else if (currPipe == 'L')
        {
            mods[2] = (-1, 1);
            newPos = (currPos.row - 1, currPos.col);
        }
        else if (currPipe == '-')
        {
            mods[0] = (-1, -1);
            mods[1] = (-1, 0);
            mods[2] = (-1, -1);
            newPos = (currPos.row, currPos.col - 1);
        }
	}
    // down
    else if (prevPos.row > currPos.row)
    {
        if (currPipe == 'F')
        {
            mods[2] = (1, 1);
            newPos = (currPos.row, currPos.col + 1);
        }
        else if (currPipe == '7')
        {
            mods[0] = (-1, 0);
            mods[1] = (0, 1);
            mods[2] = (-1, 1);
            newPos = (currPos.row, currPos.col - 1);
        }
        else if (currPipe == '|')
        {
            mods[0] = (-1, 1);
            mods[1] = (0, 1);
            mods[2] = (1, 1);
            newPos = (currPos.row - 1, currPos.col);
        }
	}
	// left
	else if (prevPos.col < currPos.col)
    {
        if (currPipe == '7')
        {
            mods[2] = (1, -1);
            newPos = (currPos.row + 1, currPos.col);
        }
        else if (currPipe == 'J')
        {
            mods[0] = (0, 1);
            mods[1] = (1, 0);
            mods[2] = (1, 1);
            newPos = (currPos.row - 1, currPos.col);
        }
        else if (currPipe == '-')
        {
            mods[0] = (1, -1);
            mods[1] = (1, 0);
            mods[2] = (1, -1);
            newPos = (currPos.row, currPos.col + 1);
        }
    }

    result[currPos.row, currPos.col] = 'X';

    foreach (var mod in mods)
    {
        var valid = true;
        valid &= (mod.row <= 0) || (mod.row > 0 && currPos.row < lines.Length - 1);
        valid &= (mod.row >= 0) || (mod.row < 0 && currPos.row > 0);
        valid &= (mod.col <= 0) || (mod.col > 0 && currPos.col < lines[0].Length - 1);
        valid &= (mod.col >= 0) || (mod.col < 0 && currPos.col > 0);

        if (valid && result[currPos.row + mod.row, currPos.col + mod.col] != 'X') result[currPos.row + mod.row, currPos.col + mod.col] = 'I';
    }

    return newPos;
}

// use fill to determine if inside or outside
bool IsInside(int row, int col, char[,] result, bool[,] marked)
{
    marked[row, col] = true;

    if (result[row, col] == 'O') return false;

    if (result[row, col] == 'I') return true;

    if (row > 0 && !marked[row - 1, col] && result[row - 1, col] != 'X') return IsInside(row - 1, col, result, marked);
    if (row < lines.Length - 1 && !marked[row + 1, col] && result[row + 1, col] != 'X') return IsInside(row + 1, col, result, marked);
    if (col > 0 && !marked[row, col - 1] && result[row, col - 1] != 'X') return IsInside(row, col - 1, result, marked);
    if (col < lines[0].Length - 1 && !marked[row, col + 1] && result[row, col + 1] != 'X') return IsInside(row, col + 1, result, marked);
    
    return true;
}
