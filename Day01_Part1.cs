using System.Text.RegularExpressions;

Console.WriteLine(File.ReadAllLines("input.txt").Select(l => int.Parse(Regex.Match(l, @"\d").Value + Regex.Match(l, @"\d", RegexOptions.RightToLeft).Value)).Sum());
