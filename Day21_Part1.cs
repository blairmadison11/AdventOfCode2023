var grid = File.ReadAllLines("input.txt").Select(l => l.ToArray()).ToArray();
var plots = new Plot[grid.Length, grid[0].Length];
Plot start = new Plot(0, 0);
for (int i = 0; i < grid.Length; ++i)
{
    for (int j = 0; j < grid[i].Length; ++j)
    {
        if (grid[i][j] == '.')
        {
            plots[i, j] = new Plot(i, j);
        }
        else if (grid[i][j] == 'S')
        {
            var p = new Plot(i, j);
            plots[i, j] = p;
            start = p;
        }
    }
}

var fourway = new (int, int)[] { (-1, 0), (0, -1), (0, 1), (1, 0) };
for (int i = 0; i < plots.GetLength(0); ++i)
{
    for (int j = 0; j < plots.GetLength(1); ++j)
    {
        if (plots[i, j] != null)
        {
            var p = plots[i, j];
            foreach (var c in fourway.Select(x => (i + x.Item1, j + x.Item2)).Where(x => x.Item1 >= 0 && x.Item1 < grid.Length && x.Item2 >= 0 && x.Item2 < grid[i].Length))
            {
                if (plots[c.Item1, c.Item2] != null)
                {
                    p.AddConnectedPlot(plots[c.Item1, c.Item2]);
                }
            }
        }
    }
}

const int LIMIT = 64;
var rem = LIMIT % 2;
var endPlots = new HashSet<Plot>();
if (rem == 0) endPlots.Add(start);

var visited = new HashSet<Plot>();
var q = new Queue<Path>();
q.Enqueue(new Path(start));
while (q.Count > 0)
{
    var path = q.Dequeue();
    for (int i = 0; i < path.NextCount; ++i)
    {
        var next = path.GetNext(i);
        var p2 = new Path(path, next);
        if (p2.Steps % 2 == rem)
        {
            endPlots.Add(next);
        }
        if (p2.Steps != LIMIT && !visited.Contains(next))
        {
            visited.Add(next);
            q.Enqueue(p2);
        }
    }
}

Console.WriteLine(endPlots.Count);

using (var sw = new StreamWriter("output.txt"))
{
    for (int i = 0; i < grid.Length; ++i)
    {
        for (int j = 0; j < grid[i].Length; ++j)
        {
            if (plots[i, j] == null)
            {
                sw.Write("#");
            }
            else if (endPlots.Contains(plots[i, j]))
            {
                sw.Write("O");
            }
            else
            {
                sw.Write(".");
            }
        }
        sw.WriteLine();
    }
}

class Path
{
    private List<Plot> plots = new List<Plot>();
    private List<Plot> adj = new List<Plot>();

    public int Steps => plots.Count - 1;

    public Path(Plot start)
    {
        plots.Add(start);
        adj = plots[0].ConnectedPlots.Where(p => !plots.Contains(p)).ToList();
    }

    public Path(Path p, Plot next)
    {
        plots = p.plots.ToList();
        plots.Add(next);
        adj = next.ConnectedPlots.Where(p => !plots.Contains(p)).ToList();
    }

    public int NextCount => adj.Count;

    public Plot GetNext(int index) => adj[index];
}

class Plot
{
    private int row, col;
    private List<Plot> connectedPlots = new List<Plot>();

    public int Row => row;
    public int Col => col;
    public int NextCount => connectedPlots.Count;
    public List<Plot> ConnectedPlots => connectedPlots;

    public Plot(int row, int col)
    {
        this.row = row;
        this.col = col;
    }

    public void AddConnectedPlot(Plot p)
    {
        if (!connectedPlots.Contains(p))
        {
            connectedPlots.Add(p);
            p.AddConnectedPlot(this);
        }
    }

    public Plot GetAdjacentPlot(int index) => connectedPlots[index];
}
