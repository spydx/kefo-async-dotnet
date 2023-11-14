using AsyncLib;

Console.WriteLine("Hello, World!");

var kefoAsyncEnumerable = new KefoAsyncEnumerable();
await kefoAsyncEnumerable.PrintRange(10, 5);

Console.WriteLine($"Main: {Environment.CurrentManagedThreadId}");