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
            Match[][] numGrid = new Match[142][];
            for (int i = 1; i < lines.Count - 1; ++i)
            {
                numGrid[i] = new Match[142];
                MatchCollection numbers = Regex.Matches(lines[i], @"\d+");
                foreach (Match number in numbers)
                {
                    for (int j = number.Index; j < number.Index + number.Length; ++j)
                    {
                        numGrid[i][j] = number;
                    }
                }
            }

            for (int i = 1; i < lines.Count - 1; ++i)
            {
                MatchCollection gears = Regex.Matches(lines[i], @"\*");
                foreach (Match gear in gears)
                {
                    Dictionary<Match, int> partNums = new Dictionary<Match, int>();
                    foreach (Match[] row in numGrid[(i - 1)..(i + 2)])
                    {
                        foreach (Match m in row[(gear.Index - 1)..(gear.Index + 2)])
                        {
                            if (m != null && !partNums.ContainsKey(m))
                            {
                                partNums[m] = int.Parse(m.Value);
                            }
                        }
                    }
                    
                    if (partNums.Count == 2)
                    {
                        int[] nums = partNums.Values.ToArray();
                        sum += nums[0] * nums[1];
                    }
                }
            }
            Console.WriteLine(sum);
        }
    }
}
