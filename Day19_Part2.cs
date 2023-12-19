using System.Text.RegularExpressions;

var lines = File.ReadAllLines(@"D:\input.txt");
var workflows = new Dictionary<string, List<Func<Dictionary<string, (int, int)>, Dictionary<string, (int, int)>>>>();
var wfcomplements = new Dictionary<string, List<Func<Dictionary<string, (int, int)>, Dictionary<string, (int, int)>>>>();
var destinations = new Dictionary<string, List<string>>();
var fallbacks = new Dictionary<string, string>();
string line;
int li = 0;
while ((line = lines[li++]) != "")
{
    var m = Regex.Match(line, @"(\w+){(?:(\w+)([\<\>])(\d+):(\w+),?)+(\w+)}");
    var src = m.Groups[1].Value;
    var fbk = m.Groups[6].Value;
    var l1 = new List<Func<Dictionary<string, (int, int)>, Dictionary<string, (int, int)>>>();
    var l2 = new List<Func<Dictionary<string, (int, int)>, Dictionary<string, (int, int)>>>();
    var l3 = new List<string>();
    for (int j = 0; j < m.Groups[2].Captures.Count; ++j)
    {
        var cat = m.Groups[2].Captures[j].Value;
        var op = m.Groups[3].Captures[j].Value;
        var num = int.Parse(m.Groups[4].Captures[j].Value);
        l3.Add(m.Groups[5].Captures[j].Value);
        if (op == "<")
        {
            l1.Add((Dictionary<string, (int, int)> p) => p[cat].Item1 < num ? (p[cat].Item2 < num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (p[cat].Item1, num - 1) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
            l2.Add((Dictionary<string, (int, int)> p) => p[cat].Item2 >= num ? (p[cat].Item1 >= num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (num, p[cat].Item2) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
        }
        else
        {
            l1.Add((Dictionary<string, (int, int)> p) => p[cat].Item2 > num ? (p[cat].Item1 > num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (num + 1, p[cat].Item2) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
            l2.Add((Dictionary<string, (int, int)> p) => p[cat].Item1 <= num ? (p[cat].Item2 <= num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (p[cat].Item1, num) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
        }
    }
    workflows.Add(src, l1);
    wfcomplements.Add(src, l2);
    destinations.Add(src, l3);
    fallbacks.Add(src, fbk);
}

var cats = new List<string>() { "x", "m", "a", "s" };
var q = new Queue<(string, Dictionary<string, (int, int)>)>();
var accs = new List<Dictionary<string, (int, int)>>();
for (int i = 0; i < workflows["in"].Count; ++i)
{
    var d = new Dictionary<string, (int, int)>();
    foreach (var c in cats)
    {
        d.Add(c, (1, 4000));
    }
    q.Enqueue(("in", d));
}

while (q.Count > 0)
{
    var r = q.Dequeue();
    if (r.Item1 == "A")
    {
        accs.Add(r.Item2);
    }
    else if (r.Item1 != "R")
    {
        var wf = workflows[r.Item1];
        var wfc = wfcomplements[r.Item1];
        var dsts = destinations[r.Item1];
        Dictionary<string, (int, int)> nextP = r.Item2;
        for (int i = 0; i < wf.Count; ++i)
        {
            var r1 = wf[i](nextP);
            q.Enqueue((dsts[i], r1));
            nextP = wfc[i](nextP);
        }
        q.Enqueue((fallbacks[r.Item1], nextP));
    }
}

ulong sum = 0;
foreach (var a in accs)
{
    ulong p = 1;
    foreach (var r in a.Values)
    {
        p *= (ulong)(r.Item2 - r.Item1) + 1;
    }
    sum += p;
}
Console.WriteLine(sum);
