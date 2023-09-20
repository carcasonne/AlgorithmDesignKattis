var N = Int32.Parse(Console.ReadLine()!);
var input = new Interval[N];
var optimal = new int[N];
Array.Fill(optimal, -1);

for(int i = 0; i < N; i++)
{
    var ints = Console.ReadLine()!
        .Split(' ')
        .Select(x => Int32.Parse(x))
        .ToList();
    var interval = new Interval(ints[0], ints[1], ints[2]);
    input[i] = interval;
}

var intervals = input.OrderBy(x => x.Finish).ToList();

var test1 = findP(intervals[7]);
var test2 = findP(intervals[6]);
var test3 = findP(intervals[1]);


Console.WriteLine(Solve(N-1));

int Solve(int i)
{
    if (i == -1)
        return 0;
    if (i == 0)
        return intervals[0].Weight;

    var o = optimal[i];
    if (o == -1)
    {
        var interval = intervals[i];
        var pValue = findP(interval);
        var keepWeight = interval.Weight + Solve(pValue);
        var dropWeight = Solve(i - 1);

        var io = Math.Max(keepWeight, dropWeight);
        optimal[i] = io;
        return io;
    }
    return o;
}

int findP(Interval interval)
{
    var indexOf = intervals.IndexOf(interval);
    for (int i = 1; i < indexOf; i++)
    {
        var newP = intervals[indexOf - i];
        if (newP.Finish <= interval.Start)
            return intervals.IndexOf(newP);
    }
    return -1;

    /*var p = reverseIntervals.FirstOrDefault(x => x.Finish <= interval.Start);
    if (p == null)
        return 0;
    var realP = intervals.IndexOf(p);
    return realP + 1;*/
}



// Console.WriteLine($"Start: {interval.Start}, finish: {interval.Finish}, Weight: {interval.Weight}");



public class Interval
{
    public int Start;
    public int Finish;
    public int Weight;

    public Interval(int s, int f, int w)
    {
        Start = s;
        Finish = f;
        Weight = w;
    }
}