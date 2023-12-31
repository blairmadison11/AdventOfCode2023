using System.Text.RegularExpressions;

var lines = File.ReadAllLines("input.txt");
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
    var wfList = new List<Func<Dictionary<string, (int, int)>, Dictionary<string, (int, int)>>>();
    var wfcList = new List<Func<Dictionary<string, (int, int)>, Dictionary<string, (int, int)>>>();
    var dstList = new List<string>();
    for (int i = 0; i < m.Groups[2].Captures.Count; ++i)
    {
        var cat = m.Groups[2].Captures[i].Value;
        var op = m.Groups[3].Captures[i].Value;
        var num = int.Parse(m.Groups[4].Captures[i].Value);
        dstList.Add(m.Groups[5].Captures[i].Value);
        if (op == "<")
        {
            wfList.Add((Dictionary<string, (int, int)> p) => p[cat].Item1 < num ? (p[cat].Item2 < num ? p.ToDictionary(e => e.Key, e => e.Value) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (p[cat].Item1, num - 1) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
            wfcList.Add((Dictionary<string, (int, int)> p) => p[cat].Item2 >= num ? (p[cat].Item1 >= num ? p.ToDictionary(e => e.Key, e => e.Value) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (num, p[cat].Item2) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
        }
        else
        {
            wfList.Add((Dictionary<string, (int, int)> p) => p[cat].Item2 > num ? (p[cat].Item1 > num ? p.ToDictionary(e => e.Key, e => e.Value) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (num + 1, p[cat].Item2) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
            wfcList.Add((Dictionary<string, (int, int)> p) => p[cat].Item1 <= num ? (p[cat].Item2 <= num ? p.ToDictionary(e => e.Key, e => e.Value) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (p[cat].Item1, num) : e.Value)) : p.ToDictionary(e => e.Key, e => e.Key == cat ? (0, 0) : e.Value));
        }
    }
    workflows.Add(src, wfList);
    wfcomplements.Add(src, wfcList);
    destinations.Add(src, dstList);
    fallbacks.Add(src, fbk);
}

var accs = new List<Dictionary<string, (int, int)>>();
var q = new Queue<(string, Dictionary<string, (int, int)>)>();
q.Enqueue(("in", new Dictionary<string, (int, int)>() { { "x", (1, 4000) }, { "m", (1, 4000) }, { "a", (1, 4000) }, { "s", (1, 4000) } }));
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

Console.WriteLine(accs.Select(a => a.Select(a => (ulong)(a.Value.Item2 - a.Value.Item1 + 1)).Aggregate((a, c) => a * c)).Aggregate((a, c) => a + c));
