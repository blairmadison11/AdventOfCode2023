using System.Text;

Path.Grid = File.ReadLines("input.txt").Select(l => l.Select(c => c - '0').ToArray()).ToArray();
Path.End = (Path.Grid.Length - 1, Path.Grid[0].Length - 1);

var adjs = new List<(int, int, Cardinal)>() { { (-1, 0, Cardinal.N) }, { (0, 1, Cardinal.E) }, { (1, 0, Cardinal.S) }, { (0, -1, Cardinal.W) } };
var lhle = int.MaxValue;
var lhl = new Dictionary<(int, int, int, Cardinal), int>();
Path bp = null;
var q = new PriorityQueue<Path, int>();
var source = new Path();
q.Enqueue(source, source.Priority);
var steps = 0;
var done = false;
while (q.Count > 0 && !done)
{
    ++steps;
    var c = q.Dequeue();
    var n = c.LastNode;
    var nextPaths = adjs.Where(x => !c.IsOppositeDir(x.Item3) && !c.IsMaxRepetition(x.Item3))
        .Select(x => (n.Item1 + x.Item1, n.Item2 + x.Item2, x.Item3))
        .Where(x => x.Item1 >= 0 && x.Item1 <= Path.End.Item1 && x.Item2 >= 0 && x.Item2 <= Path.End.Item2)
        .Select(x => new Path(c, (x.Item1, x.Item2), x.Item3))
        .Where(x => !lhl.ContainsKey((x.LastNode.Item1, x.LastNode.Item2, x.Repetitions, x.Direction))
            || x.HeatLoss < lhl[(x.LastNode.Item1, x.LastNode.Item2, x.Repetitions, x.Direction)]);
    foreach (var p in nextPaths)
    {
        lhl[(p.LastNode.Item1, p.LastNode.Item2, p.Repetitions, p.Direction)] = p.HeatLoss;

        if (p.IsComplete)
        {
            lhle = p.HeatLoss;
            bp = p;
            done = true;
            break;
        }
        else
        {
            q.Enqueue(p, p.Priority);
        }
    }
}
Console.WriteLine(lhle);

//Debug output
File.WriteAllText(@"output.txt", bp.ToString());

enum Cardinal { Unknown, N, E, S, W }
class Path
{
    public static readonly Dictionary<Cardinal, string> lookup = new Dictionary<Cardinal, string>()
    {
        { Cardinal.N, "^" },
        { Cardinal.E, ">" },
        { Cardinal.S, "v" },
        { Cardinal.W, "<" },
        { Cardinal.Unknown, "X" },
    };

    private static readonly Dictionary<Cardinal, Cardinal> opposite = new Dictionary<Cardinal, Cardinal>()
    {
        { Cardinal.N, Cardinal.S },
        { Cardinal.E, Cardinal.W },
        { Cardinal.S, Cardinal.N },
        { Cardinal.W, Cardinal.E },
        { Cardinal.Unknown, Cardinal.Unknown }
    };

    public static int[][] Grid;
    public static (int, int) End;
    public (int, int) LastNode = (0, 0), SecondLastNode = (0, 0);
    public int Repetitions = 1, HeatLoss = 0, Priority;
    public Cardinal Direction = Cardinal.Unknown;

    private Dictionary<(int, int), Cardinal> nodes = new Dictionary<(int, int), Cardinal>();

    public bool IsComplete => LastNode == Path.End;
    public bool IsOppositeDir(Cardinal dir) => dir == opposite[Direction];
    public bool Contains((int, int) node) => nodes.ContainsKey(node) || node == (0, 0);

    public Path()
    {
        Priority = (int)Math.Abs(End.Item1 - LastNode.Item1) + (int)Math.Abs(End.Item2 - LastNode.Item2);
    }

    public Path(Path p, (int, int) nextNode, Cardinal nextDir)
    {
        SecondLastNode = p.LastNode;
        LastNode = nextNode;
        nodes = p.nodes.ToDictionary(e => e.Key, e => e.Value);
        if (nodes.ContainsKey(nextNode))
        {
            nodes[nextNode] = Cardinal.Unknown;
        }
        else
        {
            nodes.Add(nextNode, nextDir);
        }
        HeatLoss = p.HeatLoss + Grid[nextNode.Item1][nextNode.Item2];
        if (nextDir == p.Direction)
        {
            Repetitions = p.Repetitions + 1;
        }
        else
        {
            Repetitions = 1;
        }
        Direction = nextDir;
        Priority = HeatLoss + (int)Math.Abs(End.Item1 - LastNode.Item1) + (int)Math.Abs(End.Item2 - LastNode.Item2);
    }

    public bool IsMaxRepetition(Cardinal dir) => dir == Direction && Repetitions == 3;
    public override string ToString()
    {
        var sb = new StringBuilder();
        for (int i = 0; i <= End.Item1; ++i)
        {
            for (int j = 0; j <= End.Item2; ++j)
            {
                sb.Append(nodes.ContainsKey((i, j)) ? lookup[nodes[(i, j)]] : Grid[i][j]);
            }
            sb.Append('\n');
        }
        return sb.ToString();
    }
}
