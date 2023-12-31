using System.Text.RegularExpressions;

var map = new Dictionary<string, (string, string)>();
var lines = File.ReadAllLines("input.txt");
var directions = lines[0].ToArray();
for (int i = 2; i < lines.Length; ++i)
{
    var m = Regex.Matches(lines[i], @"[A-Z]+");
    map.Add(m[0].Value, (m[1].Value, m[2].Value));
}

var curLoc = "AAA";
int steps = 0, dirIndex = 0;
while (curLoc != "ZZZ")
{
    curLoc = directions[dirIndex] == 'L' ? map[curLoc].Item1 : map[curLoc].Item2;
    ++steps;
    dirIndex = (dirIndex + 1) % directions.Length;
}
Console.WriteLine(steps);
