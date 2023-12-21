var grid = File.ReadAllLines("input.txt").Select(l => l.ToArray()).ToArray();
var fourway = new (int, int)[] { (-1, 0), (0, -1), (0, 1), (1, 0) };
var plots = new Plot[grid.Length, grid[0].Length];
Plot start = new Plot(0, 0);
for (int i = 0; i < grid.Length; ++i)
{
    for (int j = 0; j < grid[i].Length; ++j)
    {
        if (grid[i][j] == '.' || grid[i][j] == 'S')
        {
            Plot p = plots[i, j];
            if (p == null)
            {
                p = new Plot(i, j);
                plots[i, j] = p;
            }
         
            foreach (var c in fourway.Select(x => (i + x.Item1, j + x.Item2)).Where(x => x.Item1 >= 0 && x.Item1 < grid.Length && x.Item2 >= 0 && x.Item2 < grid[i].Length))
            {
                if (grid[c.Item1][c.Item2] == '.' || grid[c.Item1][c.Item2] == 'S')
                {
                    Plot adj = plots[c.Item1, c.Item2];
                    if (adj == null)
                    {
                        adj = new Plot(c.Item1, c.Item2);
                        plots[c.Item1, c.Item2] = adj;
                    }
                    p.AddConnectedPlot(adj);
                }
            }
            if (grid[i][j] == 'S')
            {
                start = p;
            }
        }
    }
}

const int LIMIT = 64;
var endPlots = new HashSet<Plot>();
var qp = new Queue<Plot>();
var qs = new Queue<int>();
qp.Enqueue(start);
qs.Enqueue(0);
while (qp.Count > 0)
{
    var plot = qp.Dequeue();
    var steps = qs.Dequeue() + 1;
    for (int i = 0; i < plot.NextCount; ++i)
    {
        var next = plot.GetAdjacentPlot(i);
        if (steps == LIMIT)
        {
            endPlots.Add(next);
        }
        else if (!qp.Contains(next))
        {
            qp.Enqueue(next);
            qs.Enqueue(steps);
        }
    }
}

Console.WriteLine(endPlots.Count);

class Plot
{
    private int row, col;
    private List<Plot> connectedPlots = new List<Plot>();

    public int Row => row;
    public int Col => col;
    public int NextCount => connectedPlots.Count;

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
