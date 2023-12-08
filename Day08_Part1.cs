using System.Text.RegularExpressions;

var map = new Dictionary<string, Tuple<string,string>>();
var lines = File.ReadAllLines("D:\\input.txt");
var directions = lines[0].ToArray();
for (int i = 2; i < lines.Length; ++i)
{
    var matches = Regex.Matches(lines[i], @"[A-Z]+");
    map.Add(matches[0].Value, new Tuple<string, string>(matches[1].Value, matches[2].Value));
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
