namespace TestWebApp;

public class WebFileApi(HttpClient httpClient) : IFileApi
{
    public Task<string> ReadAllText(string path) =>
        httpClient.GetStringAsync(path);
}
