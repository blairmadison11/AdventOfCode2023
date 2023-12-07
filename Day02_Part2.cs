using System.Text.RegularExpressions;

var sum = 0;
foreach (var line in File.ReadAllLines("D:\\input.txt"))
{
    var cubeCounts = new Dictionary<string, int>();
    var match = Regex.Match(line, @"Game (\d*): (?:((\d*) (red|green|blue),?\s?)+;?\s?)+");
    var counts = Array.ConvertAll(match.Groups[3].Captures.ToArray(), s => int.Parse(s.Value));
    var colors = Array.ConvertAll(match.Groups[4].Captures.ToArray(), s => s.Value);
    for (var i = 0; i < counts.Length; ++i)
    {
        if (!cubeCounts.ContainsKey(colors[i]) || cubeCounts[colors[i]] < counts[i])
        {
            cubeCounts[colors[i]] = counts[i];
        }
    }
    sum += cubeCounts.Values.Aggregate((x, y) => x * y);
}
Console.WriteLine(sum);
