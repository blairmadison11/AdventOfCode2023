using System.Text.RegularExpressions;

int total = 0;
foreach (string line in File.ReadAllLines("D:\\input.txt"))
{
    total += (int.Parse(Regex.Match(line, "\\d").Value) * 10) + int.Parse(Regex.Match(line, "\\d", RegexOptions.RightToLeft).Value);
}
Console.WriteLine(total);
