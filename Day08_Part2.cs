using System.Text.RegularExpressions;

var map = new Dictionary<string, (string, string)>();
var lines = File.ReadAllLines("input.txt");
var directions = lines[0].ToArray();
for (int i = 2; i < lines.Length; ++i)
{
    var matches = Regex.Matches(lines[i], @"[A-Z]+");
    map.Add(matches[0].Value, (matches[1].Value, matches[2].Value));
}
var curLocs = map.Keys.Where(k => k.EndsWith("A")).ToHashSet();
var stepCounts = new List<ulong>();
var dirIndex = 0;
ulong curSteps = 1;
while (curLocs.Count > 0)
{
    curLocs = curLocs.Select(c => directions[dirIndex] == 'L' ? map[c].Item1 : map[c].Item2).ToHashSet();
    foreach (var loc in curLocs.Where(c => c.EndsWith("Z")))
    {
        stepCounts.Add(curSteps);
        curLocs.Remove(loc);
    }
    dirIndex = (dirIndex + 1) % directions.Length;
    ++curSteps;
}
var gcf = (ulong a, ulong b) => { while (a != 0 && b != 0) if (a > b) a %= b; else b %= a; return a | b; };
Console.WriteLine(stepCounts.Aggregate((a, b) => (a / gcf(a, b)) * b));
