using System.Text.RegularExpressions;

var mods = new Dictionary<string, CommModule>();
foreach (var l in File.ReadAllLines("input.txt"))
{
    var m = Regex.Match(l, @"([&\%])?(\w+)\s+->\s+(?:(\w+),?\s?)+");
    var name = m.Groups[2].Value;
    var sym = m.Groups[1].Value;
    var mod = CommModule.CreateModule(name, sym);
    foreach (var dst in m.Groups[3].Captures.Select(c => c.Value))
    {
        if (mods.ContainsKey(dst))
        {
            mod.AddDestination(mods[dst]);
        }
        else
        {
            mod.AddPlaceholder(dst);
        }
    }
    mods.Add(name, mod);
}

foreach (var mod in mods.Values)
{
    mod.ProcessPlaceholders(mods);
}

var button = new Button();
for (var i = 0; i < 1000; ++i)
{
    Console.WriteLine("\nButton press #{0}:\n", i + 1);
    button.Push();
    while (CommModule.IsPropagating)
    {
        CommModule.NextModule.Tick();
    }
}

Console.WriteLine(CommModule.LowPulseCount * CommModule.HighPulseCount);

public enum Pulse { Low, High }
public abstract class CommModule
{
    private static Queue<CommModule> propagationQueue = new Queue<CommModule>();
    protected static CommModule broadcaster = null;
    protected static Dictionary<Pulse, ulong> pulseCounts = new Dictionary<Pulse, ulong>() { { Pulse.Low, 0 }, { Pulse.High, 0 } };
    public static bool IsPropagating => propagationQueue.Count > 0;
    public static CommModule NextModule => propagationQueue.Dequeue();
    public static ulong LowPulseCount => pulseCounts[Pulse.Low];
    public static ulong HighPulseCount => pulseCounts[Pulse.High];

    protected string name;
    protected List<CommModule> dsts = new List<CommModule>();
    protected Queue<(Pulse, CommModule)> buffer = new Queue<(Pulse, CommModule)>();

    private HashSet<string> placeholders = new HashSet<string>();

    public string Name => name;

    public CommModule(string name)
    {
        this.name = name;
    }

    public static CommModule CreateModule(string name, string symbol)
    {
        switch (symbol)
        {
            case "":
                var m = new Broadcaster(name);
                broadcaster = m;
                return m;
            case "%":
                return new FlipFlop(name);
            case "&":
                return new Conjunction(name);
            default:
                return null;
        }
    }

    public void AddPlaceholder(string s)
    {
        placeholders.Add(s);
    }

    public void ProcessPlaceholders(Dictionary<string, CommModule> dict)
    {
        foreach (var p in placeholders)
        {
            if (dict.ContainsKey(p))
            {
                dsts.Add(dict[p]);
                dict[p].AddSource(this);
            }
            else
            {
                dsts.Add(new Output(p));
            }
        }
        placeholders.Clear();
    }

    public void AddDestination(CommModule module)
    {
        dsts.Add(module);
        module.AddSource(this);
    }

    public void SendPulse(Pulse p, CommModule src)
    {
        buffer.Enqueue((p, src));
        propagationQueue.Enqueue(this);
        Console.WriteLine("{0}, -{1}-> {2}", src.Name, (p == Pulse.Low) ? "low" : "high", this.name);
        ++pulseCounts[p];
    }

    public virtual void Tick()
    {
        buffer.Dequeue();
    }

    public virtual void AddSource(CommModule src) { }
}

public class Broadcaster : CommModule
{
    public Broadcaster(string name) : base(name) { }

    public override void Tick()
    {
        var job = buffer.Dequeue();
        foreach (var d in dsts)
        {
            d.SendPulse(job.Item1, this);
        }
    }
}

public class FlipFlop : CommModule
{
    private bool onState = false;

    public FlipFlop(string name) : base(name) { }

    public override void Tick()
    {
        var job = buffer.Dequeue();
        if (job.Item1 == Pulse.Low)
        {
            var p2 = Pulse.Low;
            if (onState)
            {
                onState = false;
            }
            else
            {
                onState = true;
                p2 = Pulse.High;
            }
            foreach (var d in dsts)
            {
                d.SendPulse(p2, this);
            }
        }
    }
}

public class Conjunction : CommModule
{
    private Dictionary<CommModule, Pulse> memory = new Dictionary<CommModule, Pulse>();

    public Conjunction(string name) : base(name) { }

    public override void Tick()
    {
        var job = buffer.Dequeue();
        memory[job.Item2] = job.Item1;
        var p2 = (memory.Values.Where(x => x == Pulse.High).Count() == memory.Values.Count()) ? Pulse.Low : Pulse.High;
        foreach (var d in dsts)
        {
            d.SendPulse(p2, this);
        }
    }

    public override void AddSource(CommModule src)
    {
        memory.Add(src, Pulse.Low);
    }
}

class Button : CommModule
{
    public Button() : base("button") { }

    public void Push()
    {
        broadcaster.SendPulse(Pulse.Low, this);
    }
}

class Output : CommModule
{
    public Output(string s) : base(s) { }
}
