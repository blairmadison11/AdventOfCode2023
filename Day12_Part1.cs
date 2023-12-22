// works but needs optimization
using System.Text.RegularExpressions;

var puzzles = File.ReadAllLines("input.txt").Select(l => {
    var m = Regex.Match(l, @"([\#\.\?]+)\s+(?:(\d+),?)+");
    return new Puzzle(m.Groups[1].Value, m.Groups[2].Captures.Select(c => int.Parse(c.Value)).ToArray());
}).ToList();

Console.WriteLine(puzzles.Select(p => p.Solve()).Sum());

class Puzzle
{
    private Dictionary<string, int> mem = new Dictionary<string, int>();

    private string parts;
    private int[] runs;
    private int validLength;

    public override string ToString() => parts;

    public Puzzle(string input, int[] runs)
    {
        parts = Simplify(input);
        this.runs = runs;
        validLength = runs.Sum() + (runs.Length - 1);
    }

    public int Solve()
    {
        return SolveRecursive(parts);
    }

    private int SolveRecursive(string input)
    {
        if (input.Length < validLength) return 0;
        if (mem.ContainsKey(input)) return mem[input];

        var result = 0;
        var q = input.IndexOf('?');
        if (q != -1)
        {
            result += SolveRecursive(Simplify(string.Concat(input.Select((c, i) => i == q ? '#' : c))));
            result += SolveRecursive(Simplify(string.Concat(input.Select((c, i) => i == q ? '.' : c))));
        }
        else if (input.Length == validLength)
        {
            int ri = 0, run = 0;
            for (int i = 0; i <= input.Length && result == 0; ++i)
            {
                char cur = i < input.Length ? input[i] : '.';
                if (cur == '#')
                {
                    ++run;
                }
                else
                {
                    if (run == runs[ri])
                    {
                        ++ri;
                        run = 0;
                        if (ri == runs.Length)
                        {
                            if (i == input.Length)
                            {
                                result = 1;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
        mem.Add(input, result);
        return result;
    }

    private string Simplify(string input)
    {
        return input.Trim('.').Select(x => x.ToString()).Aggregate((a, c) => a + (a[^1] == '.' && c == "." ? "" : c));
    }
}
