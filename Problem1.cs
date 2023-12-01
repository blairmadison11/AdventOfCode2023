using (StreamReader reader = new StreamReader("D:\\input.txt"))
{
    int total = 0;
    while (!reader.EndOfStream)
    {
        int num = 0;
        string line = reader.ReadLine();
        for (int i = 0; i < line.Length; ++i)
        {
            char c = line[i];
            if (Char.IsDigit(c))
            {
                num = (c - '0') * 10;
                break;
            }
        }

        for (int i = line.Length - 1; i >= 0; --i)
        {
            char c = line[i];
            if (Char.IsDigit(c))
            {
                num += (c - '0');
                break;
            }
        }
        total += num;
    }
    Console.WriteLine(total);
}
