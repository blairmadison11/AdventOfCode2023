// This BFS solution works with the sample data but is too slow for the full input
using System.Text;

Path.Grid = File.ReadLines(@"D:\input.txt").Select(l => l.Select(c => c - '0').ToArray()).ToArray();
Path.End = (Path.Grid.Length - 1, Path.Grid[0].Length - 1);

var adjs = new List<(int, int, Cardinal)>() { { (-1, 0, Cardinal.N) }, { (0, 1, Cardinal.E) }, { (1, 0, Cardinal.S) }, { (0, -1, Cardinal.W) } };
var lhle = int.MaxValue;
var lhl = new Dictionary<(int,int,int), int>();
Path bp = null;
var s = new Queue<Path>();
s.Enqueue(new Path());
while (s.Count > 0)
{
    var c = s.Dequeue();
    var n = c.LastNode;
    var nextPaths = adjs.Where(x => !c.IsMaxRepetition(x.Item3))
        .Select(x => (n.Item1 + x.Item1, n.Item2 + x.Item2, x.Item3))
        .Where(x => x.Item1 >= 0 && x.Item1 <= Path.End.Item1 && x.Item2 >= 0 && x.Item2 <= Path.End.Item2 && !c.Contains((x.Item1, x.Item2)))
        .Select(x => new Path(c, (x.Item1, x.Item2), x.Item3))
        .Where(x => !lhl.ContainsKey((x.LastNode.Item1, x.LastNode.Item2, x.Repetitions)) || x.HeatLoss <= lhl[(x.LastNode.Item1, x.LastNode.Item2, x.Repetitions)]);
    foreach (var p in nextPaths)
    {
        lhl[(p.LastNode.Item1, p.LastNode.Item2, p.Repetitions)] = p.HeatLoss;
        
        if (p.IsComplete)
        {
            if (p.HeatLoss < lhle)
            {
                lhle = p.HeatLoss;
                bp = p;
            }    
        }
        else
        {
            s.Enqueue(p);
        }
    }
}
Console.WriteLine(lhle);
File.WriteAllText(@"D:\output.txt", bp.ToString());

enum Cardinal { Unknown, N, E, S, W }
class Path
{
    public static readonly Dictionary<Cardinal, string> lookup = new Dictionary<Cardinal, string>()
    {
        { Cardinal.N, "^" },
        { Cardinal.E, ">" },
        { Cardinal.S, "v" },
        { Cardinal.W, "<" }
    };

    public static int[][] Grid;
    public static (int, int) End;
    public (int, int) LastNode = (0, 0), SecondLastNode = (0, 0);
    public int Repetitions = 1, HeatLoss = 0;

    private Dictionary<(int, int), Cardinal> nodes = new Dictionary<(int, int), Cardinal>();
    private Cardinal curDir = Cardinal.Unknown;
    
    public bool IsComplete => LastNode == Path.End;
    public bool Contains((int, int) node) => nodes.ContainsKey(node) || node == (0, 0);

    public Path() { }
    public Path(Path p, (int, int) node, Cardinal dir)
    {
        SecondLastNode = p.LastNode;
        LastNode = node;
        nodes = p.nodes.ToDictionary(e => e.Key, e => e.Value);
        nodes.Add(node, dir);
        HeatLoss = p.HeatLoss + Grid[node.Item1][node.Item2];
        if (dir == p.curDir)
        {
            Repetitions = p.Repetitions + 1;
        }
        else
        {
            Repetitions = 1;
        }
        curDir = dir;
    }

    public bool IsMaxRepetition(Cardinal dir) => dir == curDir && Repetitions >= 3;
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
