using System.Text.RegularExpressions;

var vals = File.ReadAllLines("D:\\input.txt").Select(l => long.Parse(String.Join("", Array.ConvertAll(Regex.Match(l, @"(?:(\d+)\s*)+").Groups[1].Captures.ToArray(), c => c.Value)))).ToArray();
Console.WriteLine((int)((-vals[0] + Math.Sqrt(Math.Pow(vals[0], 2) - (4 * vals[1]))) / 2) - (int)((-vals[0] - Math.Sqrt(Math.Pow(vals[0], 2) - (4 * vals[1]))) / 2));
