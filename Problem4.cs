using System.Text.RegularExpressions;

namespace Program
{
    public class Program
    {
        static string PATTERN1 = @"Game (\d*): (?:([^;]*);?\s?)+",
            PATTERN2 = @"((\d*) (red|green|blue),?\s?)+";

        public static void Main(string[] args)
        {
            int powerSum = 0;
            using (StreamReader reader = new StreamReader("D:\\input.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Dictionary<string, int> cubeCounts = new Dictionary<string, int>();
                    Match match1 = Regex.Match(line, PATTERN1);
                    foreach (Capture capture in match1.Groups[2].Captures)
                    {
                        Match match2 = Regex.Match(capture.Value, PATTERN2);
                        CaptureCollection counts = match2.Groups[2].Captures;
                        CaptureCollection colors = match2.Groups[3].Captures;
                        for (int i = 0; i < colors.Count; ++i)
                        {
                            int count = int.Parse(counts[i].Value);
                            string color = colors[i].Value;
                            if (!cubeCounts.ContainsKey(colors[i].Value) || cubeCounts[color] < count)
                            {
                                cubeCounts[color] = count;
                            }
                        }
                    }
                    int power = 1;
                    foreach (int count in cubeCounts.Values)
                    {
                        power *= count;
                    }
                    powerSum += power;
                }
            }
            Console.WriteLine(powerSum);
        }
    }
}
