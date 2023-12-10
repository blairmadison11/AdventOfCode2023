var lines = new List<string>(File.ReadAllLines("D:\\input2.txt").Select(line => string.Format(".{0}", line)));
var x = lines[0].Length;
lines.Insert(0, new string('.', x));

var pipes = new Pipe[x][];
Pipe start = null;
for (var i = 0; i < lines.Count; ++i)
{
    pipes[i] = new Pipe[lines[i].Length];
    for (var j = 1; j < lines[i].Length; ++j)
    {
        char c = lines[i][j];
        if (c != '.')
        {
            var pipe = new Pipe(c);
            foreach (Cardinal card in pipe.AvailableCardinals)
            {
                if (card == Cardinal.N && pipes[i - 1][j] != null)
                {
                    pipe.AddConnection(pipes[i - 1][j], card);
                }
                else if (card == Cardinal.W && pipes[i][j - 1] != null)
                {
                    pipe.AddConnection(pipes[i][j - 1], card);
                }
            }
            pipes[i][j] = pipe;
            if (c == 'S')
            {
                start = pipe;
            }
        }
    }
}

var sc = start.ConnectedCardinals;
var paths = new List<List<Pipe>>();
paths.Add(new List<Pipe>() { start, start.GetConnection(sc[0]) });
paths.Add(new List<Pipe>() { start, start.GetConnection(sc[1]) });
while (paths[0][^1] != paths[1][^1] && paths[0][^1] != paths[1][^2])
{
    foreach (var path in paths)
    {
        path.Add(path[^1].GetNext());
    }
}
Console.WriteLine(paths[0].Count - 1);


enum Cardinal { N, E, S, W };
class Pipe
{
    private static readonly Dictionary<char, Cardinal[]> lookup = new Dictionary<char, Cardinal[]>()
    {
        { '|', new [] { Cardinal.N, Cardinal.S } },
        { '-', new [] { Cardinal.E, Cardinal.W } },
        { 'L', new [] { Cardinal.N, Cardinal.E } },
        { 'J', new [] { Cardinal.N, Cardinal.W } },
        { '7', new [] { Cardinal.S, Cardinal.W } },
        { 'F', new [] { Cardinal.S, Cardinal.E } },
        { 'S', new [] { Cardinal.N, Cardinal.E, Cardinal.S, Cardinal.W } }
    };

    private static readonly Dictionary<Cardinal, Cardinal> opposite = new Dictionary<Cardinal, Cardinal>()
    {
        { Cardinal.N, Cardinal.S },
        { Cardinal.E, Cardinal.W },
        { Cardinal.S, Cardinal.N },
        { Cardinal.W, Cardinal.E }
    };

    public Cardinal Prev;

    private Cardinal[] cardinals;
    private Dictionary<Cardinal, Pipe> connections = new Dictionary<Cardinal, Pipe>();

    public Pipe(char c)
    {
        cardinals = lookup[c];
    }

    public Cardinal[] AvailableCardinals => cardinals;
    public Cardinal[] ConnectedCardinals => connections.Keys.ToArray();
    public bool HasCardinal(Cardinal c) => cardinals.Contains(c);

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

    public Pipe GetNext()
    {
        foreach (Cardinal c in connections.Keys)
        {
            if (c != Prev)
            {
                return GetConnection(c);
            }
        }
        return null;
    }
}
