using System.Text.RegularExpressions;

var sum = 0;
var maxVals = new Dictionary<string, int>() {{"red", 12},{"green", 13},{"blue", 14}};
foreach (var line in File.ReadAllLines("D:\\input.txt"))
{
    var match = Regex.Match(line, @"Game (\d*): (?:((\d*) (red|green|blue),?\s?)+;?\s?)+");
    var counts = Array.ConvertAll(match.Groups[3].Captures.ToArray(), s => int.Parse(s.Value));
    var colors = Array.ConvertAll(match.Groups[4].Captures.ToArray(), s => s.Value);
    var possibleFlag = true;
    for (var i = 0; i < counts.Length; ++i)
    {
        if (counts[i] > maxVals[colors[i]])
        {
            possibleFlag = false;
            break;
        }
    }
    if (possibleFlag) sum += int.Parse(match.Groups[1].Value);
}
Console.WriteLine(sum);
