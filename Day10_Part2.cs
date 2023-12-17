// took me a week to come up with this janky solution, but it works and it's 100% my own (no help or hints received)
var lines = new List<string>(File.ReadAllLines(@"D:\input.txt").Select(line => string.Format(".{0}", line)));
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

// find the golden path
var path = new List<Pipe>() { start, start.GetConnection(start.ConnectedCardinals[0])};
Pipe p;
while ((p = path[^1].GetNext()) != path[0])
{
    path.Add(p);
}

// plot the path on a grid
var pathGrid = new Pipe[pipes.GetLength(0) + 1, pipes.GetLength(1) + 1];
foreach (var pipe in path)
{
    pathGrid[pipe.X, pipe.Y] = pipe;
}

// find all adjacent empty spaces on each side of the path
var sideA = new HashSet<(int, int)>();
var sideB = new HashSet<(int, int)>();
foreach (var pipe in path)
{
    sideA.UnionWith(pipe.GetAdjacentCoords(pathGrid, false));
    sideB.UnionWith(pipe.GetAdjacentCoords(pathGrid, true));
}

// find out which side is inside and which side is outside
var surround = new List<(int, int)>() { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
var sideAisOut = false;
foreach (var c in sideA)
{
    var visited = new bool[pathGrid.GetLength(0), pathGrid.GetLength(1)];
    var s = new Stack<(int, int)>();
    s.Push(c);
    while (s.Count > 0 && !sideAisOut)
    {
        var cc = s.Pop();
        if (!visited[cc.Item1, cc.Item2])
        {
            visited[cc.Item1, cc.Item2] = true;
            foreach (var cc2 in surround.Select(x => (x.Item1 + cc.Item1, x.Item2 + cc.Item2))
                    .Where(y => y.Item1 >= 0 && y.Item1 < pathGrid.GetLength(0) && y.Item2 >= 0 && y.Item2 < pathGrid.GetLength(1)
                    && pathGrid[y.Item1, y.Item2] == null))
            {
                if (cc2.Item1 == 0 || cc2.Item1 == pathGrid.GetLength(0) - 1 || cc2.Item2 == 0 || cc2.Item2 == pathGrid.GetLength(1) - 1)
                {
                    sideAisOut = true;
                    break;
                }
                s.Push(cc2);
            }
        }
    }
    if (sideAisOut) break;
}

// search for all empty spaces adjacent to inside edge
var inside = sideAisOut ? sideB : sideA;
var insCoords = new HashSet<(int, int)>();
foreach (var c in inside)
{
    var s = new Stack<(int, int)>();
    if (!insCoords.Contains(c))
    {
        s.Push(c);
        while (s.Count > 0)
        {
            var cc = s.Pop();
            if (!insCoords.Contains(cc))
            {
                insCoords.Add(cc);
                foreach (var cc2 in surround.Select(x => (x.Item1 + cc.Item1, x.Item2 + cc.Item2))
                    .Where(y => y.Item1 >= 0 && y.Item1 < pathGrid.GetLength(0) && y.Item2 >= 0 && y.Item2 < pathGrid.GetLength(1)
                    && pathGrid[y.Item1, y.Item2] == null))
                {
                    s.Push(cc2);
                }
            }
        }
    }
}
Console.WriteLine(insCoords.Count());

// START DEBUG OUTPUT
using (StreamWriter sw = new StreamWriter("D:\\output.txt"))
{
    for (int i = 0; i < pathGrid.GetLength(0); ++i)
    {
        for (int j = 0; j < pathGrid.GetLength(1); ++j)
        {
            if (sideA.Contains((i, j)))
            {
                sw.Write('A');
            }
            else if (sideB.Contains((i, j)))
            {
                sw.Write('B');
            }
            else if (pathGrid[i, j] != null)
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
    sw.WriteLine();
}
// END DEBUG OUTPUT

enum Cardinal { N, E, S, W };
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

    private static readonly Dictionary<char, int> sideLookup = new Dictionary<char, int>()
    {
        { '│', 90 },
        { '─', 0 },
        { '└', 45 },
        { '┘', 315 },
        { '┐', 225 },
        { '┌', 135 },
        { 'X', 0 }
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
    public int SideAngle;

    private (int, int) coords;
    private Cardinal[] cardinals;
    private Dictionary<Cardinal, Pipe> connections = new Dictionary<Cardinal, Pipe>();
    

    public Pipe(char c, (int, int) coords)
    {
        Symbol = symLookup[c];
        cardinals = cardLookup[c];
        SideAngle = sideLookup[Symbol];
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

        // the code below is  voodoo and even though I wrote it I barely understand why it works
        if (Math.Abs(this.SideAngle - p.SideAngle) == 90 || Math.Abs(this.SideAngle - p.SideAngle) == 270)
        {
            if (this.SideAngle == sideLookup[this.Symbol] && p.SideAngle == sideLookup[p.Symbol])
            {
                p.SideAngle = (this.SideAngle + 270) % 360;
            }
            else
            {
                p.SideAngle = (this.SideAngle + 90) % 360;
            }
        }
        else if (Math.Abs((this.SideAngle - (p.SideAngle + 360)) % 360) > 90 && Math.Abs(((this.SideAngle + 360) - p.SideAngle) % 360) > 90)
        {
            p.SideAngle = (p.SideAngle + 180) % 360;
        }
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

    public HashSet<(int,int)> GetAdjacentCoords(Pipe[,] grid, bool opposite)
    {
        var l = new List<(int,int)>();
        var angle = this.SideAngle;
        if (opposite)
        {
            angle = (angle + 180) % 360;
        }
        switch (angle)
        {
            case 0:
                l.Add((X - 1, Y));
                break;
            case 45:
                l.Add((X - 1, Y + 1));
                if (Symbol == '┐')
                {
                    l.Add((X - 1, Y));
                    l.Add((X, Y + 1));
                }
                break;
            case 90:
                l.Add((X, Y + 1));
                break;
            case 135:
                l.Add((X + 1, Y + 1));
                if (Symbol == '┘')
                {
                    l.Add((X + 1, Y));
                    l.Add((X, Y + 1));
                }
                break;
            case 180:
                l.Add((X + 1, Y));
                break;
            case 225:
                l.Add((X + 1, Y - 1));
                if (Symbol == '└')
                {
                    l.Add((X, Y - 1));
                    l.Add((X + 1, Y));
                }
                break;
            case 270:
                l.Add((X, Y - 1));
                break;
            case 315:
                l.Add((X - 1, Y - 1));
                if (Symbol == '┌')
                {
                    l.Add((X - 1, Y));
                    l.Add((X, Y - 1));
                }
                break;
        }
        var t = l.Where(c => grid[c.Item1, c.Item2] == null).ToHashSet();
        return t;
    }
}
