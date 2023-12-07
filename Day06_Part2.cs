using System.Text.RegularExpressions;

var lines = File.ReadAllLines("D:\\input.txt");
var time = long.Parse(Array.ConvertAll(Regex.Match(lines[0], @"Time:\s+(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => c.Value).Aggregate((x, y) => x + y));
var dist = long.Parse(Array.ConvertAll(Regex.Match(lines[1], @"Distance:\s+(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => c.Value).Aggregate((x, y) => x + y));
var wins = 0;
for (var btn = 0; btn < time; ++btn)
{
    if ((time - btn) * btn > dist)
    {
        ++wins;
    }
}
Console.WriteLine(wins);
