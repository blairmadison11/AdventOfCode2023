// Yes, this solution is brute force and inelegant but it's the best I can do for now
using System.Text.RegularExpressions;

var maps = new List<Func<long, long>>();
var lines = File.ReadAllLines("D:\\input.txt");
var seeds = Array.ConvertAll(Regex.Match(lines[0], @"seeds: (?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => long.Parse(c.Value));
for (var i = 1; i < seeds.Length; i += 2)
{
    seeds[i] = seeds[i - 1] + seeds[i];
}

for (var i = lines.Length - 1; i > 1; --i)
{
    if (lines[i].Contains("map"))
    {
        var ranges = new List<long[]>();
        for (var j = i + 1; j < lines.Length && lines[j] != ""; ++j)
        {
            ranges.Add(Array.ConvertAll(lines[j].Split(' '), s => long.Parse(s)));
        }
        maps.Add((long x) => { foreach (var range in ranges) if (x >= range[0] && x < range[0] + range[2]) return x + (range[1] - range[0]); return x; });
    }
}

var foundFlag = false;
for (long i = 0; i < long.MaxValue && !foundFlag; ++i)
{
    var x = i;
    foreach (var map in maps)
    {
        x = map(x);
    }
    for (var j = 0; j < seeds.Length && !foundFlag; j += 2)
    {
        if (x >= seeds[j] && x < seeds[j + 1])
        {
            Console.WriteLine(i);
            foundFlag = true;
        }
    }
}
