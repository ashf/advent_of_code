<Query Kind="Statements" />

var lines = File.ReadLines("c:/dev/advent_of_code/2023/day7/input.txt");

var sortedHands = lines
    .Select(line => new Hand(
        cards: line.Split(' ')[0],
        bid: int.Parse(line.Split(' ')[1])))
    .OrderDescending();

var totalWinnings = 0;
int rank = 1;
foreach (var hand in sortedHands)
{
    totalWinnings += hand.Bid * rank;
    rank++;
}

totalWinnings.Dump();


class Hand : IComparable<Hand>
{
    string _cards;
    public int Bid;
    public HandType HandType;

    static Dictionary<char, int> s_cardToValue = new Dictionary<char, int>
    {
        { '2', 2 },
        { '3', 3 },
        { '4', 4 },
        { '5', 5 },
        { '6', 6 },
        { '7', 7 },
        { '8', 8 },
        { '9', 9 },
        { 'T', 10 },
        { 'J', 11 },
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 }
    };

    public Hand(string cards, int bid)
    {
        _cards = cards;
        Bid = bid;
        HandType = CalcHandType();
    }

    public int CompareTo(Hand other)
    {        
        if (this.HandType < other.HandType) return -1;
     
        if (this.HandType > other.HandType) return 1;
        
        for (int i = 0; i < 5; i++)
        {
            if (s_cardToValue[this._cards[i]] > s_cardToValue[other._cards[i]]) return -1;
            if (s_cardToValue[this._cards[i]] < s_cardToValue[other._cards[i]]) return 1;
        }
        
        return 0;
    }
    
    HandType CalcHandType()
    {
        Dictionary<char, int> cardCount = new();
        foreach (var card in _cards)
        {
            if (!cardCount.ContainsKey(card))
            {
                cardCount[card] = 1;
            }
            else
            {
                cardCount[card]++;                
            }
        }
        
        var orderedCardCount = cardCount.OrderByDescending(x => x.Value);
        var first = orderedCardCount.First();
        
        if (first.Value == 5) return HandType.FiveOfAKind;
        if (first.Value == 4) return HandType.FourOfAKind;
        if (first.Value == 1) return HandType.HighCard;
        
        var second = orderedCardCount.Skip(1).First();

        if (first.Value == 3)
        {
            if (second.Value == 2) return HandType.FullHouse;
            
            return HandType.ThreeOfAKind;
        }
        
        return (second.Value == 2)
            ? HandType.TwoPair
            : HandType.OnePair;
    }
}

enum HandType
{
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeOfAKind,
    TwoPair,
    OnePair,
    HighCard
}