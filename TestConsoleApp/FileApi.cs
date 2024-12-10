// ReSharper disable once CheckNamespace
namespace MinecraftDataCSharp;

public class FileApi : IFileApi
{
    public Task<string> ReadAllText(string path) =>
        File.ReadAllTextAsync(path);
}
