using System.Text.RegularExpressions;

var hands = new List<Hand>();
foreach (string line in File.ReadAllLines("D:\\input.txt"))
{
    var m = Regex.Match(line, @"([2-9TJQKA])+ (\d+)");
    hands.Add(new Hand(m.Groups[1].Captures.Select(c => c.Value).ToArray(), int.Parse(m.Groups[2].Value)));
}
hands.Sort();
var sum = 0;
for (var i = 0; i < hands.Count; ++i)
{
    sum += hands[i].Bid * (i + 1);
}
Console.WriteLine(sum);

class Hand : IComparable<Hand>
{
    private static readonly Dictionary<string, int> lookup = new Dictionary<string, int>() {
        {"2", 1}, {"3", 2}, {"4", 3}, {"5", 4}, {"6", 5}, {"7", 6}, {"8", 7},
        {"9", 8}, {"T", 9}, {"J", 0}, {"Q", 10}, {"K", 11}, {"A", 12} };

    private int[] cards;
    private int rank, bid;
    public Hand(string[] cardSymbols, int bid)
    {
        this.bid = bid;
        cards = cardSymbols.Select(c => lookup[c]).ToArray();
        var counts = new Dictionary<int, int>();
        var numJokers = 0;
        foreach (int card in cards)
        {
            if (card == 0)
            {
                ++numJokers;
            }
            else
            {
                counts[card] = counts.GetValueOrDefault(card) + 1;
            }
        }
        rank = counts.Count > 0 ? (counts.Values.Max() + numJokers) - counts.Count : 4;
    }

    public int GetCard(int i) => cards[i];
    public int Bid => bid;
    public int Rank => rank;

    public int CompareTo(Hand? other)
    {
        if (other == null) return 0;

        if (this.Rank == other.Rank)
        {
            for (int i = 0; i < 5; ++i)
            {
                if (this.GetCard(i) != other.GetCard(i))
                {
                    return this.GetCard(i).CompareTo(other.GetCard(i));
                }
            }
            return 0;
        }
        return this.Rank.CompareTo(other.Rank);
    }
}
