using System.Text.RegularExpressions;

var lines = File.ReadAllLines("D:\\input.txt");
var time = long.Parse(Array.ConvertAll(Regex.Match(lines[0], @"Time:\s+(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => c.Value).Aggregate((x, y) => x + y));
var dist = long.Parse(Array.ConvertAll(Regex.Match(lines[1], @"Distance:\s+(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => c.Value).Aggregate((x, y) => x + y));
Console.WriteLine(((-time + Math.Round(Math.Sqrt(Math.Pow(time, 2) - (4 * dist)))) / 2) - ((-time - Math.Round(Math.Sqrt(Math.Pow(time, 2) - (4 * dist)))) / 2));
