using System.Text.RegularExpressions;

var pointSum = 0;
foreach (string line in File.ReadAllLines("D:\\input.txt"))
{
    var parse = Regex.Match(line, @"Card\s+\d+:\s+((\d+)\s*)+\|\s+((\d+)\s*)+");
    var winningNums = new HashSet<int>(Array.ConvertAll(parse.Groups[2].Captures.ToArray(), s => int.Parse(s.Value)));
    var scratchNums = new HashSet<int>(Array.ConvertAll(parse.Groups[4].Captures.ToArray(), s => int.Parse(s.Value)));
    pointSum += (int)Math.Pow(2, scratchNums.Intersect(winningNums).Count() - 1);
}
Console.WriteLine(pointSum);
