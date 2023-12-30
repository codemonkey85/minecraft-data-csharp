namespace MinecraftDataCSharp;

public interface IFileApi
{
    Task<string> ReadAllText(string path);
}
