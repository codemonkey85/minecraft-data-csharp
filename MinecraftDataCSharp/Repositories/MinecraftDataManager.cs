namespace MinecraftDataCSharp.Repositories;

public class MinecraftDataManager(DataPathResolver pathResolver)
{
    private readonly DataPathResolver pathResolver = pathResolver;
    
    public string Edition { get; private set; } = Constants.DefaultEdition;

    public string Version { get; private set; } = Constants.DefaultPcVersion;

    public void SetVersion(string newVersion) => Version = newVersion;

    public void SetEdition(string newEdition)
    {
        Edition = newEdition;
        Version = newEdition == "pc"
            ? Constants.DefaultPcVersion
            : Constants.DefaultBedrockVersion; // Default per edition
    }

    public string? GetFilePath(string category) => pathResolver.GetFilePath(Edition, Version, category);
}
