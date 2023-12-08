using System.Text.RegularExpressions;

var hands = File.ReadAllLines("D:\\input.txt").Select(l => {
        var m = Regex.Match(l, @"([2-9TJQKA])+ (\d+)");
        return new Hand(m.Groups[1].Captures.Select(c => c.Value).ToArray(), int.Parse(m.Groups[2].Value));
    }).ToList();
hands.Sort();
Console.WriteLine(hands.Select((h, i) => h.Bid * (i + 1)).Sum());

class Hand : IComparable<Hand>
{
    private static readonly Dictionary<string, int> lookup = new Dictionary<string, int>() {
        {"2", 0}, {"3", 1}, {"4", 2}, {"5", 3}, {"6", 4}, {"7", 5}, {"8", 6},
        {"9", 7}, {"T", 8}, {"J", 9}, {"Q", 10}, {"K", 11}, {"A", 12} };

    private int[] cards;
    private int rank, bid;
    public Hand(string[] cardSymbols, int bid)
    {
        this.bid = bid;
        cards = cardSymbols.Select(c => lookup[c]).ToArray();
        var counts = new Dictionary<int, int>();
        foreach (int card in cards)
        {
            counts[card] = counts.GetValueOrDefault(card) + 1;
        }
        rank = counts.Values.Max() - counts.Count;
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
