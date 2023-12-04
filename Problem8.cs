using System.Text.RegularExpressions;

string[] lines = File.ReadAllLines("D:\\input.txt");
int[] cards = Enumerable.Repeat(1, lines.Length).ToArray();
for (int i = 0; i < lines.Length; i++)
{
    Match parse = Regex.Match(lines[i], @"Card\s+\d+:\s+((\d+)\s*)+\|\s+((\d+)\s*)+");
    HashSet<int> winningNums = new HashSet<int>(Array.ConvertAll(parse.Groups[2].Captures.ToArray(), s => int.Parse(s.Value)));
    HashSet<int> scratchNums = new HashSet<int>(Array.ConvertAll(parse.Groups[4].Captures.ToArray(), s => int.Parse(s.Value)));
    int points = scratchNums.Intersect(winningNums).Count();
    for (int j = i + 1; j < i + points + 1; ++j)
    {
        cards[j] += cards[i];
    }
}
Console.WriteLine(cards.Sum());
