namespace MinecraftDataCSharp.Repositories;

public class MinecraftDataManager(DataPathResolver pathResolver)
{
    // ReSharper disable once MemberCanBePrivate.Global
    public string Edition { get; private set; } = Editions.Pc;

    // ReSharper disable once MemberCanBePrivate.Global
    public string Version { get; private set; } = PcVersions.Latest;

    public void SetVersion(string newVersion) => Version = newVersion;

    public void SetEdition(string newEdition)
    {
        Edition = newEdition;
        Version = newEdition == Editions.Pc
            ? PcVersions.Latest
            : BedrockVersions.Latest; // Default per edition
    }

    public string? GetFilePath(string category) => pathResolver.GetFilePath(Edition, Version, category);
}
