using System.Text.RegularExpressions;

Console.WriteLine(File.ReadAllLines("D:\\input.txt").Select(l => int.Parse(string.Concat(Regex.Match(l, @"\d").Value, Regex.Match(l, @"\d", RegexOptions.RightToLeft).Value))).Sum());
