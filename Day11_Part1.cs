var grid = File.ReadAllLines("input.txt").Select(l => l.ToList()).ToList();
var blankRows = Enumerable.Range(0, grid.Count).ToList();
var blankCols = Enumerable.Range(0, grid[0].Count).ToList();
for (int i = 0; i < grid.Count; ++i)
{
    for (int j = 0; j < grid[i].Count; ++j)
    {
        if (grid[i][j] == '#')
        {
            blankRows.Remove(i);
            blankCols.Remove(j);
        }
    }
}

for (int i = 0; i < grid.Count; ++i)
{
    var colInc = 0;
    for (int j = 0; j < blankCols.Count; ++j)
    {
        grid[i].Insert(blankCols[j] + colInc++, '.');
    }
}

var rowInc = 0;
for (int i = 0; i < blankRows.Count; ++i)
{
    grid.Insert(blankRows[i] + rowInc++, Enumerable.Repeat('.', grid[0].Count).ToList());
}

var vertices = new List<Vertex>();
for (int i = 0; i < grid.Count; ++i)
{
    for (int j = 0; j < grid[i].Count; ++j)
    {
        if (grid[i][j] == '#')
        {
            vertices.Add(new Vertex(i, j));
        }
    }
}

foreach (var v in vertices)
{
    v.AddConnections(vertices);
}

var sum = 0;
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
    private Dictionary<Vertex, int> connections;

    public Vertex(int x, int y)
    {
        this.coords = (x, y);
    }

    public int X => coords.Item1;
    public int Y => coords.Item2;

    public void AddConnections(List<Vertex> vertices)
    {
        connections = new Dictionary<Vertex, int>();
        foreach (var v in vertices)
        {
            if (v != this)
            {
                var dist = (int)Math.Abs(v.X - this.X) + (int)Math.Abs(v.Y - this.Y);
                connections.Add(v, dist);
            }
        }
    }

    public int GetDistanceTo(Vertex v) => connections[v];
}
