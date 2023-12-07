using System.Text.RegularExpressions;

var sum = 0;
var maxVals = new Dictionary<string, int>() {{"red", 12},{"green", 13},{"blue", 14}};
foreach (var line in File.ReadAllLines("D:\\input.txt"))
{
    var match = Regex.Match(line, @"Game (\d*): (?:((\d*) (red|green|blue),?\s?)+;?\s?)+");
    var counts = match.Groups[3].Captures.Select(s => int.Parse(s.Value));
    var colors = match.Groups[4].Captures.Select(s => s.Value);
    var possibleFlag = true;
    for (var i = 0; i < counts.Length && possibleFlag; ++i)
    {
        if (counts[i] > maxVals[colors[i]])
        {
            possibleFlag = false;
        }
    }
    if (possibleFlag) sum += int.Parse(match.Groups[1].Value);
}
Console.WriteLine(sum);
