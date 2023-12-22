using System.Text.RegularExpressions;

Console.WriteLine(File.ReadAllLines("input.txt").Select(l => {
    var m = Regex.Match(l, @"([\#\.\?]+)\s+(?:(\d+),?)+");
    return new Puzzle(m.Groups[1].Value, m.Groups[2].Captures.Select(c => int.Parse(c.Value)).ToArray());
}).Select(p => p.Solve()).Sum());

class Puzzle
{
    private Dictionary<string, int> mem = new Dictionary<string, int>();

    private string parts;
    private string valid;

    public Puzzle(string input, int[] runs)
    {
        parts = Simplify(input);
        valid = string.Join('.', runs.Select(r => new string('#', r)));
    }

    private string Simplify(string input) => input.Trim('.').Select(x => x.ToString()).Aggregate((a, c) => a[^1] == '.' && c == "." ? a : a + c);
    public int Solve() => SolveRecursive(parts);

    private int SolveRecursive(string input)
    {
        if (input.Length < valid.Length) return 0;
        if (mem.ContainsKey(input)) return mem[input];

        var result = 0;
        var q = input.IndexOf('?');
        if (q != -1)
        {
            result += SolveRecursive(Simplify(string.Concat(input.Select((c, i) => i == q ? '#' : c))));
            result += SolveRecursive(Simplify(string.Concat(input.Select((c, i) => i == q ? '.' : c))));
        }
        else if (input == valid)
        {
            result = 1;
        }
        mem.Add(input, result);
        return result;
    }
}
