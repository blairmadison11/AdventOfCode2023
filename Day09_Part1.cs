using System.Text.RegularExpressions;

var hists = File.ReadAllLines("D:\\input.txt").Select(l => Regex.Match(l, @"(?:(-?\d+)\s*)+").Groups[1].Captures.Select(c => long.Parse(c.Value)).ToList()).ToList();
var nextVals = new List<long>();
foreach (var hist in hists)
{
    var seqs = new List<List<long>>();
    seqs.Add(hist);
    var done = false;
    while (!done)
    {
        var seq = new List<long>();
        for (int i = 1; i < seqs[^1].Count; ++i)
        {
            seq.Add(seqs[^1][i] - seqs[^1][i - 1]);
        }
        seqs.Add(seq);
        if (seq.Where(s => s == 0).Count() == seq.Count)
        {
            done = true;
        }
    }
    seqs[^1].Add(0);
    for (int i = seqs.Count - 2; i >= 0; --i)
    {
        seqs[i].Add(seqs[i][^1] + seqs[i + 1][^1]);
    }
    nextVals.Add(seqs[0][^1]);
}
Console.WriteLine(nextVals.Sum());
