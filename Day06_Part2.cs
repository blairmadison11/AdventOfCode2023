using System.Text.RegularExpressions;

var vals = File.ReadAllLines("D:\\input2.txt").Select(l => long.Parse(String.Join("", Regex.Match(l, @"(?:(\d+)\s*)+").Groups[1].Captures.Select(c => c.Value)))).ToArray();
Console.WriteLine((long)((-vals[0] + Math.Sqrt(Math.Pow(vals[0], 2) - (4 * vals[1]))) / 2) - (long)(-vals[0] - Math.Sqrt(Math.Pow(vals[0], 2) - (4 * vals[1]))) / 2);
