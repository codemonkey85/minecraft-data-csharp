namespace MinecraftDataCSharp.Repositories;

public class MinecraftDataManager(DataPathResolver pathResolver)
{
    private readonly DataPathResolver pathResolver = pathResolver;
    private string edition = "pc"; // Default edition
    private string version = "1.20"; // Default PC version

    public void SetVersion(string newVersion) => version = newVersion;

    public void SetEdition(string newEdition)
    {
        edition = newEdition;
        version = newEdition == "pc"
            ? "1.20"
            : "1.20"; // Default per edition
    }

    public string? GetFilePath(string category) => pathResolver.GetFilePath(edition, version, category);
}
