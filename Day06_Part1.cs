using System.Text.RegularExpressions;

var lines = File.ReadAllLines("D:\\input.txt");
var times = Array.ConvertAll(Regex.Match(lines[0], @"Time:\s+(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => int.Parse(c.Value));
var dists = Array.ConvertAll(Regex.Match(lines[1], @"Distance:\s+(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => int.Parse(c.Value));
var wins = new List<int>();
for (var i = 0; i < times.Length; ++i)
{
    var curWins = 0;
    for (var btn = 0; btn < times[i]; ++btn)
    {
        if ((times[i] - btn) * btn > dists[i])
        {
            ++curWins;
        }
    }
    wins.Add(curWins);
}
Console.WriteLine(wins.Aggregate((x, y) => x * y));
