using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var locs = Regex.Matches(lines[0], @"\d+").Select(c => long.Parse(c.Value)).ToArray();
for (var i = 2; i < lines.Length; ++i)
{
    var rs = new List<long[]>();
    for (++i; i < lines.Length && lines[i] != ""; ++i)
    {
        rs.Add(lines[i].Split(' ').Select(s => long.Parse(s)).ToArray());
    }
    locs = locs.Select(l => { foreach (var r in rs) if (l >= r[1] && l < r[1] + r[2]) return l + (r[0] - r[1]); return l; }).ToArray();
}
Console.WriteLine(locs.Min());
