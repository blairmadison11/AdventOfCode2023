using System.Text.RegularExpressions;

var sum = 0;
var lines = new List<string>();
lines.AddRange(File.ReadAllLines("D:\\input.txt").Select(line => string.Format(".{0}.", line)));
lines.Insert(0, new string('.', lines[0].Length));
lines.Add(new string('.', lines[0].Length));
for (var i = 1; i < lines.Count - 1; ++i)
{
    foreach (Match match in Regex.Matches(lines[i], @"\d+"))
    {
        int x1 = match.Index - 1, x2 = match.Index + match.Length + 1;
        if (Regex.IsMatch(lines[i - 1][x1..x2], @"[^.\d]") ||
            Regex.IsMatch(lines[i + 1][x1..x2], @"[^.\d]") ||
                lines[i][x1] != '.' || lines[i][x2 - 1] != '.')
        {
            sum += int.Parse(match.Value);
        }
    }
}
Console.WriteLine(sum);
