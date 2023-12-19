using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var times = Regex.Match(lines[0], @"Time:\s+(?:(\d+)\s*)+").Groups[1].Captures.Select(c => int.Parse(c.Value));
var dists = Regex.Match(lines[1], @"Distance:\s+(?:(\d+)\s*)+").Groups[1].Captures.Select(c => int.Parse(c.Value));
var wins = new List<int>();
for (var i = 0; i < times.Length; ++i)
{
    var root1 = (int)((-times[i] + Math.Sqrt(Math.Pow(times[i], 2.0) - (4 * dists[i]))) / 2);
    var root2 = (int)((-times[i] - Math.Sqrt(Math.Pow(times[i], 2.0) - (4 * dists[i]))) / 2);
    wins.Add(root1 - root2);
}
Console.WriteLine(wins.Aggregate((x, y) => x * y));
