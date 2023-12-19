using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var cards = Enumerable.Repeat(1, lines.Length).ToArray();
for (var i = 0; i < lines.Length; i++)
{
    var m = Regex.Match(lines[i], @"Card\s+\d+:\s+((\d+)\s*)+\|\s+((\d+)\s*)+");
    var p = m.Groups[4].Captures.Select(s => s.Value).Intersect(m.Groups[2].Captures.Select(s => s.Value)).Count();
    for (var j = i + 1; j < i + p + 1; ++j)
    {
        cards[j] += cards[i];
    }
}
Console.WriteLine(cards.Sum());
