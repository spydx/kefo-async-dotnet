using System.Runtime.CompilerServices;

namespace AsyncLib;

public class KefoAsyncEnumerable()
{
    public async Task PrintRange(int start, int count)
    {
        var cts = new CancellationTokenSource();

        await foreach (var item in IntegerListAsync(start, count).WithCancellation(cts.Token))
        {
            Console.WriteLine($"PrintRange: {Environment.CurrentManagedThreadId}");
            Console.WriteLine(item);
        }
        Console.WriteLine($"PrintRange: {Environment.CurrentManagedThreadId}");
    }

    private async IAsyncEnumerable<int> IntegerListAsync(int start, int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var range = Enumerable.Range(start, count).Reverse();
        foreach (var i in range)
        {
            Console.WriteLine($"ListRange: {Environment.CurrentManagedThreadId}");
            var k = i * 100;
            await Task.Delay(k, cancellationToken);
            Console.WriteLine($"ListRange: {Environment.CurrentManagedThreadId}");
            yield return k;
        }
    }
}