// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, World!");

Console.WriteLine(KefoPlinq.GetThreadId());

var kefoPlinq = new KefoPlinq();
kefoPlinq.GetRange()
    .AsParallel()
    // .AsOrdered()
    .Select(x =>
    {
        KefoPlinq.WaitBeforeReturn(x).GetAwaiter().GetResult();
        return x;
    })
    .WithDegreeOfParallelism(20)
    .Aggregate(seed: new List<int>(), (acc, result) =>
    {
        Console.WriteLine($"RangeID: {result}");
        acc.Add(result);
        return acc;
    });

kefoPlinq.GetRange()
    .AsParallel()
    .WithDegreeOfParallelism(20)
    .Select(task =>
    {
        KefoPlinq.WaitBeforeReturn(task).GetAwaiter().GetResult();
        return task;
    })
    .ForAll(x =>
    {
        Console.WriteLine($"RangeID: {x} : ThreadID: {Environment.CurrentManagedThreadId}");
    });


public class KefoPlinq
{
    private static readonly Random _random = new();
    public IEnumerable<int> GetRange()
    {
        return Enumerable.Range(1, 20);
    }

    public static Task WaitBeforeReturn(int x)
    {
        var time = _random.Next(20);
        var timeout = time * 100;
        Console.WriteLine($"RangeId: {x} : {GetThreadId()} : Timeout: {timeout}");
        return Task.Delay(timeout);
    }

    public static string GetThreadId()
    {
        return $"Thread ID: {Environment.CurrentManagedThreadId}";
    }
}