namespace AsyncMain;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        const string url = "https://httpstat.us/200";

        var httpClient = new HttpClient();
        var response = await httpClient
            .GetAsync(url);

        var content = response.Content
            .ReadAsStringAsync();

        Console.WriteLine(content);
    }
}
