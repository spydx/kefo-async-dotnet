namespace AsyncLib;

public class KefoAsync(HttpClient httpClient)
{
    public async Task<string> GetData(string url)
    {
        Console.WriteLine($"GetRequest: {Environment.CurrentManagedThreadId}");
        return await GetRequest(url);
    }

    private async Task<string> GetRequest(string url)
    {
        var getRequest = HttpGetAsync(url);
        Console.WriteLine($"GetRequest: {Environment.CurrentManagedThreadId}");
        await Task.Delay(2000);

        Console.WriteLine($"GetRequest: {Environment.CurrentManagedThreadId}");
        var content = await getRequest;
        var doctype = content[..10];
        Console.WriteLine($"GetRequest: {Environment.CurrentManagedThreadId}");

        return $"Awaited: {doctype}";
    }

    private async Task<string> HttpGetAsync(string url)
    {
        var response = await httpClient.GetAsync(url);
        Console.WriteLine($"HTTPGetAsync: {Environment.CurrentManagedThreadId}");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"HTTPGetAsync: {Environment.CurrentManagedThreadId}");
        return content;
    }

    private async Task<string> CreateString()
    {
        await Task.Delay(1000);
        return "hi";
    }
}