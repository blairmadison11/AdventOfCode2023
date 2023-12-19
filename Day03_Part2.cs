using System.Text.RegularExpressions;

var sum = 0;
var lines = new List<string>(File.ReadAllLines("input.txt").Select(line => string.Format(".{0}.", line)));
lines.Insert(0, new string('.', lines[0].Length));
lines.Add(new string('.', lines[0].Length));

var grid = new Match[lines.Count, lines[0].Length];
for (var i = 1; i < lines.Count - 1; ++i)
{
    foreach (Match number in Regex.Matches(lines[i], @"\d+"))
    {
        for (var j = number.Index; j < number.Index + number.Length; ++j)
        {
            grid[i, j] = number;
        }
    }
}

for (var i = 1; i < lines.Count - 1; ++i)
{
    foreach (Match gear in Regex.Matches(lines[i], @"\*"))
    {
        Dictionary<Match, int> partNums = new Dictionary<Match, int>();
        for (var j = i - 1; j <= i + 1; ++j)
        {
            for (var k = gear.Index - 1; k <= gear.Index + 1; ++k)
            {
                var m = grid[j, k];
                if (m != null && !partNums.ContainsKey(m))
                {
                    partNums[m] = int.Parse(m.Value);
                }
            }
        }
        if (partNums.Count == 2) sum += partNums.Values.Aggregate((x, y) => x * y);
    }
}
Console.WriteLine(sum);
