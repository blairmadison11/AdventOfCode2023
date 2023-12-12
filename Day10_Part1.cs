var lines = new List<string>(File.ReadAllLines("D:\\input.txt").Select(line => string.Format(".{0}", line)));
lines.Insert(0, new string('.', lines[0].Length));

var pipes = new Pipe[lines.Count, lines[0].Length];
Pipe start = null;
for (var i = 0; i < lines.Count; ++i)
{
    for (var j = 1; j < lines[i].Length; ++j)
    {
        char c = lines[i][j];
        if (c != '.')
        {
            var pipe = new Pipe(c, (i, j));
            foreach (Cardinal card in pipe.AvailableCardinals)
            {
                if (card == Cardinal.N && pipes[i - 1, j] != null)
                {
                    pipe.AddConnection(pipes[i - 1, j], card);
                }
                else if (card == Cardinal.W && pipes[i, j - 1] != null)
                {
                    pipe.AddConnection(pipes[i, j - 1], card);
                }
            }
            pipes[i, j] = pipe;
            if (c == 'S')
            {
                start = pipe;
            }
        }
    }
}

var pathGrid = new Pipe[pipes.GetLength(0) + 1, pipes.GetLength(1) + 1];
var sc = start.ConnectedCardinals;
var paths = new List<List<Pipe>>();
paths.Add(new List<Pipe>() { start, start.GetConnection(sc[0]) });
paths.Add(new List<Pipe>() { start, start.GetConnection(sc[1]) });
while (paths[0][^1] != paths[1][^1] && paths[0][^1] != paths[1][^2])
{
    foreach (var path in paths)
    {
        Pipe p = path[^1].GetNext();
        pathGrid[p.X, p.Y] = p;
        path.Add(p);
    }
}
Console.WriteLine(paths[0].Count - 1);

// DEBUG OUTPUT
using (StreamWriter sw = new StreamWriter("D:\\output.txt"))
{
    for (int i = 0; i < pathGrid.GetLength(0); ++i)
    {
        for (int j = 0; j < pathGrid.GetLength(1); ++j)
        {
            if (pathGrid[i, j] != null)
            {
                sw.Write(pathGrid[i, j].Symbol);
            }
            else
            {
                sw.Write('.');
            }
        }
        sw.WriteLine();
    }
}

enum Cardinal { N, NE, E, SE, S, SW, W, NW };
class Pipe
{
    private static readonly Dictionary<char, Cardinal[]> cardLookup = new Dictionary<char, Cardinal[]>()
    {
        { '|', new [] { Cardinal.N, Cardinal.S } },
        { '-', new [] { Cardinal.E, Cardinal.W } },
        { 'L', new [] { Cardinal.N, Cardinal.E } },
        { 'J', new [] { Cardinal.N, Cardinal.W } },
        { '7', new [] { Cardinal.S, Cardinal.W } },
        { 'F', new [] { Cardinal.S, Cardinal.E } },
        { 'S', new [] { Cardinal.N, Cardinal.E, Cardinal.S, Cardinal.W } }
    };

    private static readonly Dictionary<char, char> symLookup = new Dictionary<char, char>()
    {
        { '|', '│' },
        { '-', '─' },
        { 'L', '└' },
        { 'J', '┘' },
        { '7', '┐' },
        { 'F', '┌' },
        { 'S', 'X' }
    };

    private static readonly Dictionary<Cardinal, Cardinal> opposite = new Dictionary<Cardinal, Cardinal>()
    {
        { Cardinal.N, Cardinal.S },
        { Cardinal.E, Cardinal.W },
        { Cardinal.S, Cardinal.N },
        { Cardinal.W, Cardinal.E }
    };

    public char Symbol;
    public Cardinal Prev;
    public Cardinal[] sides = new Cardinal[2];

    private (int, int) coords;
    private Cardinal[] cardinals;
    private Dictionary<Cardinal, Pipe> connections = new Dictionary<Cardinal, Pipe>();

    public Pipe(char c, (int, int) coords)
    {
        Symbol = symLookup[c];
        cardinals = cardLookup[c];
        this.coords = coords;
    }

    public Cardinal[] AvailableCardinals => cardinals;
    public Cardinal[] ConnectedCardinals => connections.Keys.ToArray();
    
    public bool HasCardinal(Cardinal c) => cardinals.Contains(c);
    public bool IsVertical => connections.Keys.Contains(Cardinal.N) || connections.Keys.Contains(Cardinal.S);
    public bool IsHorizontal => connections.Keys.Contains(Cardinal.W) || connections.Keys.Contains(Cardinal.E);
    public int X => coords.Item1;
    public int Y => coords.Item2;

    public void AddConnection(Pipe p, Cardinal c)
    {
        if (!connections.ContainsKey(c) && p.HasCardinal(opposite[c]))
        {
            connections[c] = p;
            p.AddConnection(this, opposite[c]);
        }
    }

    public Pipe GetConnection(Cardinal c)
    {
        Pipe p = connections[c];
        p.Prev = opposite[c];
        return p;
    }

    public Cardinal GetOppositeDirection(Cardinal card)
    {
        foreach (Cardinal c in connections.Keys)
        {
            if (c != Prev)
            {
                return c;
            }
        }
        return card;
    }

    public Pipe GetNext() => GetConnection(GetOppositeDirection(Prev));

    public override string ToString() => coords.ToString();
}
