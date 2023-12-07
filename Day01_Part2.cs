using System.Text.RegularExpressions;

var lookup = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };
var eval = (string m) => Char.IsDigit(m[0]) ? m[0] - '0' : lookup[m];
var p = @"\d|one|two|three|four|five|six|seven|eight|nine";
Console.WriteLine(File.ReadLines("D:\\input.txt").Select(l => eval(Regex.Match(l, p).Value) * 10 + eval(Regex.Match(l, p, RegexOptions.RightToLeft).Value)).Aggregate((x, y) => x + y));
