using System.Text.RegularExpressions;

var digits = new Dictionary<string, int>() { { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 } };
var GetNum = (string m) => Char.IsDigit(m[0]) ? m[0] - '0' : digits[m];
var pattern = @"\d|one|two|three|four|five|six|seven|eight|nine";
Console.WriteLine(File.ReadLines("D:\\input.txt").Select(l => GetNum(Regex.Match(l, pattern).Value) * 10 + GetNum(Regex.Match(l, pattern, RegexOptions.RightToLeft).Value)).Aggregate((x,y) => x + y));
