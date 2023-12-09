using System.Text.RegularExpressions;

Console.WriteLine(File.ReadAllLines("D:\\input.txt").Select(l => (int.Parse(Regex.Match(l, "\\d").Value) * 10) + int.Parse(Regex.Match(l, "\\d", RegexOptions.RightToLeft).Value)).Sum());
