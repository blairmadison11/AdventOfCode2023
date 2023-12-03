using System.Text.RegularExpressions;

namespace Program
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<string> lines = new List<string>();
            lines.Add(new string('.', 142));
            using (StreamReader reader = new StreamReader("D:\\input.txt"))
            {
                while (!reader.EndOfStream)
                {
                    lines.Add(string.Format(".{0}.", reader.ReadLine()));
                }
            }
            lines.Add(new string('.', 142));

            int sum = 0;
            for (int i = 1; i < lines.Count - 1; ++i)
            {
                MatchCollection matches = Regex.Matches(lines[i], @"\d+");
                foreach (Match match in matches)
                {
                    int startX = match.Index - 1, length = match.Length + 2, endX = match.Index + match.Length;
                    if (Regex.IsMatch(lines[i - 1].Substring(startX, length), @"[^.\d]") ||
                        Regex.IsMatch(lines[i + 1].Substring(startX, length), @"[^.\d]") ||
                            lines[i][startX] != '.' || lines[i][endX] != '.')
                    {
                        sum += int.Parse(match.Value);
                    }
                }
            }
            Console.WriteLine(sum);
        }
    }
}
