// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

namespace Program
{
    public class Program
    {
        static string AOC_PATTERN = @"\d|one|two|three|four|five|six|seven|eight|nine";
        static Dictionary<string, int> DIGIT_WORDS = new Dictionary<string, int>()
        {
            {"one", 1 },
            {"two", 2 },
            {"three", 3 },
            {"four", 4 },
            {"five", 5 },
            {"six", 6 },
            {"seven", 7 },
            {"eight", 8 },
            {"nine", 9 }
        };

        public static void Main(string[] args)
        {
            int total = 0;
            using (StreamReader reader = new StreamReader("D:\\input.txt"))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    Match leftMatch = Regex.Match(line, AOC_PATTERN);
                    Match rightMatch = Regex.Match(line, AOC_PATTERN, RegexOptions.RightToLeft);
                    total += (GetNumberFromMatch(leftMatch) * 10) + GetNumberFromMatch(rightMatch);
                }
            }
            Console.WriteLine(total);
        }

        public static int GetNumberFromMatch(Match match)
        {
            if (Char.IsDigit(match.Value[0]))
            {
                return match.Value[0] - '0';
            }
            else
            {
                return DIGIT_WORDS[match.Value];
            }
        }
    }
}





