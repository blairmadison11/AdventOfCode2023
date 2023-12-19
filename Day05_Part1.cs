using System.Text.RegularExpressions;

var maps = new List<Func<long,long>>();
var lines = File.ReadAllLines("input.txt");
var seeds = Array.ConvertAll(Regex.Match(lines[0], @"seeds: (?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => long.Parse(c.Value));
for (var i = 2; i < lines.Length; ++i)
{
    if (lines[i].Contains("map"))
    {
        var ranges = new List<long[]>();
        for (var j = i + 1; j < lines.Length && lines[j] != ""; ++j)
        {
            ranges.Add(Array.ConvertAll(lines[j].Split(' '), s => long.Parse(s)));
        }
        maps.Add((long x) => { foreach (var range in ranges) if (x >= range[1] && x < range[1] + range[2]) return x + (range[0] - range[1]); return x; });
    }
}

var locs = new List<long>();
foreach (var seed in seeds)
{
    var loc = seed;
    foreach (var map in maps)
    {
        loc = map(loc);
    }
    locs.Add(loc);
}
Console.WriteLine(locs.Min());
