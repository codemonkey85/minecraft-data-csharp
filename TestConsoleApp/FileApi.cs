namespace MinecraftDataCSharp;

public class FileApi : IFileApi
{
    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }
}
