<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day4/input.txt").ToList();

int cardNum = 0;
var cards = lines.ToDictionary(k => cardNum++, v => 1);

var cardResult = new Dictionary<int, int>();

var cardsProcessed = 0;

cardNum = 0;
while (cardNum < lines.Count)
{
	cardsProcessed++;
	cards[cardNum]--;
	
	if (!cardResult.ContainsKey(cardNum))
	{
		var line = lines[cardNum];

		var firstSplit = line.Split('|');
		var winners = firstSplit[0].Split(':')[1].Trim().Split(' ').Where(x => x != "");
		var guesses = firstSplit[1].Trim().Split(' ').Where(x => x != "");

		cardResult[cardNum] = winners.Intersect(guesses).Count();
	}

	for (int index = cardNum + 1; index <= cardNum + cardResult[cardNum]; index++)
	{
		cards[index]++;
	}
	
	cardNum = cards[cardNum] == 0 ? cardNum + 1 : cardNum;
}

cardsProcessed.Dump();