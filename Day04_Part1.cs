using System.Text.RegularExpressions;

Console.WriteLine(File.ReadAllLines("input.txt").Select(l => {
    var m = Regex.Match(l, @"Card\s+\d+:\s+((\d+)\s*)+\|\s+((\d+)\s*)+");
    return (int)Math.Pow(2, m.Groups[4].Captures.Select(s => s.Value).Intersect(m.Groups[2].Captures.Select(s => s.Value)).Count() - 1);
}).Sum());
