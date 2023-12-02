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
                    Game game = new Game();
                    Match match1 = Regex.Match(line, PATTERN1);
                    foreach (Capture capture in match1.Groups[2].Captures)
                    {
                        
                        Match match2 = Regex.Match(capture.Value, PATTERN2);
                        CaptureCollection counts = match2.Groups[2].Captures;
                        CaptureCollection colors = match2.Groups[3].Captures;
                        for (int i = 0; i < colors.Count; ++i)
                        {
                            game.UpdateMin(int.Parse(counts[i].Value), colors[i].Value);
                        }
                    }
                    powerSum += game.GetPower();
                }
            }
            Console.WriteLine(powerSum);
        }
    }

    public class Game
    {
        private Dictionary<string, int> cubeCounts = new Dictionary<string, int>();

        public void UpdateMin(int count, string color)
        {
            if (cubeCounts.ContainsKey(color))
            {
                int current = cubeCounts[color];
                if (count > current)
                {
                    cubeCounts[color] = count;
                }
            }
            else
            {
                cubeCounts[color] = count;
            }
        }

        public int GetPower()
        {
            int power = 1;
            foreach (string color in cubeCounts.Keys)
            {
                power *= cubeCounts[color];
            }
            return power;
        }
    }
}
