var grid = File.ReadAllLines("input.txt").Select(l => l.ToArray()).ToArray();
var blankRows = Enumerable.Range(0, grid.Length).ToHashSet();
var blankCols = Enumerable.Range(0, grid[0].Length).ToHashSet();
for (int i = 0; i < grid.Length; ++i)
{
    for (int j = 0; j < grid[i].Length; ++j)
    {
        if (grid[i][j] == '#')
        {
            blankRows.Remove(i);
            blankCols.Remove(j);
        }
    }
}

var colWidths = Enumerable.Repeat((ulong)1, grid[0].Length).ToArray();
var rowWidths = Enumerable.Repeat((ulong)1, grid.Length).ToArray();
foreach (var col in blankCols)
{
    colWidths[col] = 1000000;
}

foreach (var row in blankRows)
{
    rowWidths[row] = 1000000;
}

var vertices = new List<Vertex>();
for (int i = 0; i < grid.Length; ++i)
{
    for (int j = 0; j < grid[i].Length; ++j)
    {
        if (grid[i][j] == '#')
        {
            vertices.Add(new Vertex(i, j));
        }
    }
}

foreach (var v in vertices)
{
    v.AddConnections(vertices, rowWidths, colWidths);
}

ulong sum = 0;
for (int i = 0; i < vertices.Count; ++i)
{
    for (int j = i + 1; j < vertices.Count; ++j)
    {
        sum += vertices[i].GetDistanceTo(vertices[j]);
    }
}
Console.WriteLine(sum);

class Vertex
{
    private (int, int) coords;
    private Dictionary<Vertex, ulong> connections;

    public Vertex(int x, int y)
    {
        this.coords = (x, y);
    }

    public int X => coords.Item1;
    public int Y => coords.Item2;

    public void AddConnections(List<Vertex> vertices, ulong[] rowWidths, ulong[] colWidths)
    {
        connections = new Dictionary<Vertex, ulong>();
        foreach (var v in vertices)
        {
            if (v != this)
            {
                var rowSpan = rowWidths[Math.Min(v.X, X)..Math.Max(v.X, X)];
                var colSpan = colWidths[Math.Min(v.Y, Y)..Math.Max(v.Y, Y)];
                var rowSum = rowSpan.Length > 0 ? rowSpan.Aggregate((a, b) => a + b) : 0;
                var colSum = colSpan.Length > 0 ? colSpan.Aggregate((a, b) => a + b) : 0;
                connections.Add(v, rowSum + colSum);
            }
        }
    }

    public ulong GetDistanceTo(Vertex v) => connections[v];
}
