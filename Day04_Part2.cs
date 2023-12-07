using System.Text.RegularExpressions;

var lines = File.ReadAllLines("D:\\input.txt");
var cards = Enumerable.Repeat(1, lines.Length).ToArray();
for (var i = 0; i < lines.Length; i++)
{
    var parse = Regex.Match(lines[i], @"Card\s+\d+:\s+((\d+)\s*)+\|\s+((\d+)\s*)+");
    var winningNums = new HashSet<int>(parse.Groups[2].Captures.Select(s => int.Parse(s.Value)));
    var scratchNums = new HashSet<int>(parse.Groups[4].Captures.Select(s => int.Parse(s.Value)));
    var points = scratchNums.Intersect(winningNums).Count();
    for (var j = i + 1; j < i + points + 1; ++j)
    {
        cards[j] += cards[i];
    }
}
Console.WriteLine(cards.Sum());
