using System.Text.RegularExpressions;

using (StreamReader reader = new StreamReader("D:\\input.txt"))
{
    List<int> points = new List<int>();
    while (!reader.EndOfStream)
    {
        Match parse = Regex.Match(reader.ReadLine(), @"Card\s+\d+:\s+((\d+)\s*)+\|\s+((\d+)\s*)+");
        HashSet<int> winningNums = new HashSet<int>(Array.ConvertAll(parse.Groups[2].Captures.ToArray(), s => int.Parse(s.Value)));
        HashSet<int> scratchNums = new HashSet<int>(Array.ConvertAll(parse.Groups[4].Captures.ToArray(), s => int.Parse(s.Value)));
        points.Add(scratchNums.Intersect(winningNums).Count());
    }

    int[] cards = Enumerable.Repeat(1, points.Count).ToArray();
    for (int i = 0; i < points.Count; ++i)
    {
        for (int j = i + 1; j < i + 1 + points[i]; ++j)
        {
            cards[j] += cards[i];
        }
    }
    Console.WriteLine(cards.Sum());
}
