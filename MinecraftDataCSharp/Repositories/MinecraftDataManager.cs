namespace MinecraftDataCSharp.Repositories;

public class MinecraftDataManager(DataPathResolver pathResolver)
{
    private readonly DataPathResolver pathResolver = pathResolver;
    private string edition = Constants.DefaultEdition;
    private string version = Constants.DefaultPcVersion;

    public void SetVersion(string newVersion) => version = newVersion;

    public void SetEdition(string newEdition)
    {
        edition = newEdition;
        version = newEdition == "pc"
            ? Constants.DefaultPcVersion
            : Constants.DefaultBedrockVersion; // Default per edition
    }

    public string? GetFilePath(string category) => pathResolver.GetFilePath(edition, version, category);
}
