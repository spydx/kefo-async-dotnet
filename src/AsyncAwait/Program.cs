// See https://aka.ms/new-console-template for more information

using AsyncLib;

Console.WriteLine("Hello, World!");


const string url = "https://www.kefo.no";

Console.WriteLine($"Main: {Environment.CurrentManagedThreadId}");
using var httpClient = new HttpClient();

var kefoAsync = new KefoAsync(httpClient);

var message = await kefoAsync.GetData(url);
Console.WriteLine($"Main: {Environment.CurrentManagedThreadId}");
Console.WriteLine(message);