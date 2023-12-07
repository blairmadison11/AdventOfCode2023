using System.Text.RegularExpressions;

var sum = 0;
var lines = new List<string>(File.ReadAllLines("D:\\input.txt").Select(line => string.Format(".{0}.", line)));
var x = lines[0].Length;
lines.Insert(0, new string('.', x));
lines.Add(new string('.', x));

var grid = new Match[x][];
for (int i = 1; i < lines.Count - 1; ++i)
{
    grid[i] = new Match[x];
    foreach (Match number in Regex.Matches(lines[i], @"\d+"))
    {
        for (int j = number.Index; j < number.Index + number.Length; ++j)
        {
            grid[i][j] = number;
        }
    }
}

for (int i = 1; i < lines.Count - 1; ++i)
{
    foreach (Match gear in Regex.Matches(lines[i], @"\*"))
    {
        Dictionary<Match, int> partNums = new Dictionary<Match, int>();
        foreach (Match[] row in grid[(i - 1)..(i + 2)])
        {
            foreach (Match m in row[(gear.Index - 1)..(gear.Index + 2)])
            {
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
