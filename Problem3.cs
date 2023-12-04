using System.Text.RegularExpressions;

int idSum = 0;
Dictionary<string, int> MAX_VALUES = new Dictionary<string, int>()
{
    { "red", 12 },
    { "green", 13 },
    { "blue", 14 }
};

using (StreamReader reader = new StreamReader("D:\\input.txt"))
{
    while (!reader.EndOfStream)
    {
        bool possibleFlag = true;
        string line = reader.ReadLine();
        Match match1 = Regex.Match(line, @"Game (\d*): (?:([^;]*);?\s?)+");
        foreach (Capture capture in match1.Groups[2].Captures)
        {
            Match match2 = Regex.Match(capture.Value, @"((\d*) (red|green|blue),?\s?)+");
            CaptureCollection counts = match2.Groups[2].Captures;
            CaptureCollection colors = match2.Groups[3].Captures;
            for (int i = 0; i < colors.Count; ++i)
            {
                int count = int.Parse(counts[i].Value);
                if (count > MAX_VALUES[colors[i].Value])
                {
                    possibleFlag = false;
                    break;
                }
            }
            if (!possibleFlag)
            {
                break;
            }
        }
        if (possibleFlag)
        {
            idSum += int.Parse(match1.Groups[1].Value);
        }
    }
}
Console.WriteLine(idSum);
