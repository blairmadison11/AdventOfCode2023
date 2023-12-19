using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var workflows = new Dictionary<string, List<Func<Dictionary<string, int>, string>>>();
var fallbacks = new Dictionary<string, string>();
var parts = new List<Dictionary<string, int>>();
string line;
int i = 0;
while ((line = lines[i++]) != "")
{
    var m = Regex.Match(line, @"(\w+){(?:(\w+)([\<\>])(\d+):(\w+),?)+(\w+)}");
    var src = m.Groups[1].Value;
    var fbk = m.Groups[6].Value;
    fallbacks.Add(src, fbk);
    var l = new List<Func<Dictionary<string, int>, string>>();
    for (int j = 0; j < m.Groups[2].Captures.Count; ++j)
    {
        var cat = m.Groups[2].Captures[j].Value;
        var op = m.Groups[3].Captures[j].Value;
        var num = int.Parse(m.Groups[4].Captures[j].Value);
        var dst = m.Groups[5].Captures[j].Value;
        
        if (op == "<")
        {
            l.Add((Dictionary<string, int> p) => p[cat] < num ? dst : "");
        }
        else
        {
            l.Add((Dictionary<string, int> p) => p[cat] > num ? dst : "");
        }
    }
    workflows.Add(src, l);
}
while (i < lines.Length)
{
    var m = Regex.Match(lines[i], @"{(?:([xmas])=(\d+),?)+}");
    var d = new Dictionary<string, int>();
    for (int j = 0; j < m.Groups[1].Captures.Count; ++j)
    {
        var cat = m.Groups[1].Captures[j].Value;
        var num = int.Parse(m.Groups[2].Captures[j].Value);
        d.Add(cat, num);
    }
    parts.Add(d);
    ++i;
}

var sum = 0;
foreach (var p in parts)
{
    string w = "in";
    while (w != "R" && w != "A")
    {
        var found = false;
        foreach (var f in workflows[w])
        {
            var r = f(p);
            if (r != "")
            {
                w = r;
                found = true;
                break;
            }
        }
        if (!found)
        {
            w = fallbacks[w];
        }
    }
    if (w == "A")
    {
        sum += p.Values.Sum();
    }
}
Console.WriteLine(sum);
