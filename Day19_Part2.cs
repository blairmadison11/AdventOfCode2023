using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
var workflows = new Dictionary<string, List<Func<Dictionary<string, (ulong, ulong)>, Dictionary<string, (ulong, ulong)>>>>();
var wfcomplements = new Dictionary<string, List<Func<Dictionary<string, (ulong, ulong)>, Dictionary<string, (ulong, ulong)>>>>();
var destinations = new Dictionary<string, List<string>>();
var fallbacks = new Dictionary<string, string>();
string line;
int li = 0;
while ((line = lines[li++]) != "")
{
    var m = Regex.Match(line, @"(\w+){(?:(\w+)([\<\>])(\d+):(\w+),?)+(\w+)}");
    var src = m.Groups[1].Value;
    var fbk = m.Groups[6].Value;
    var wfList = new List<Func<Dictionary<string, (ulong, ulong)>, Dictionary<string, (ulong, ulong)>>>();
    var wfcList = new List<Func<Dictionary<string, (ulong, ulong)>, Dictionary<string, (ulong, ulong)>>>();
    var dstList = new List<string>();
    for (int j = 0; j < m.Groups[2].Captures.Count; ++j)
    {
        var cat = m.Groups[2].Captures[j].Value;
        var op = m.Groups[3].Captures[j].Value;
        var num = ulong.Parse(m.Groups[4].Captures[j].Value);
        dstList.Add(m.Groups[5].Captures[j].Value);
        if (op == "<")
        {
            wfList.Add((Dictionary<string, (ulong, ulong)> p) => p[cat].Item1 < num ? (p[cat].Item2 < num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (p[cat].Item1, num - 1) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
            wfcList.Add((Dictionary<string, (ulong, ulong)> p) => p[cat].Item2 >= num ? (p[cat].Item1 >= num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (num, p[cat].Item2) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
        }
        else
        {
            wfList.Add((Dictionary<string, (ulong, ulong)> p) => p[cat].Item2 > num ? (p[cat].Item1 > num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (num + 1, p[cat].Item2) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
            wfcList.Add((Dictionary<string, (ulong, ulong)> p) => p[cat].Item1 <= num ? (p[cat].Item2 <= num ? p : p.ToDictionary(e => e.Key, e => e.Key == cat ? (p[cat].Item1, num) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
        }
    }
    workflows.Add(src, wfList);
    wfcomplements.Add(src, wfcList);
    destinations.Add(src, dstList);
    fallbacks.Add(src, fbk);
}

var accs = new List<Dictionary<string, (ulong, ulong)>>();
var q = new Queue<(string, Dictionary<string, (ulong, ulong)>)>();
q.Enqueue(("in", new Dictionary<string, (ulong, ulong)>() { { "x", (1, 4000) }, { "m", (1, 4000) }, { "a", (1, 4000) }, { "s", (1, 4000) } }));
while (q.Count > 0)
{
    var p = q.Dequeue();
    if (p.Item1 == "A")
    {
        accs.Add(p.Item2);
    }
    else if (p.Item1 != "R")
    {
        var wf = workflows[p.Item1];
        var wfc = wfcomplements[p.Item1];
        var dsts = destinations[p.Item1];
        var next = p.Item2;
        for (int i = 0; i < wf.Count; ++i)
        {
            var p2 = wf[i](next);
            q.Enqueue((dsts[i], p2));
            next = wfc[i](next);
        }
        q.Enqueue((fallbacks[p.Item1], next));
    }
}

Console.WriteLine(accs.Select(a => a.Select(a => a.Value.Item2 - a.Value.Item1 + 1).Aggregate((a, c) => a * c)).Aggregate((a, c) => a + c));
