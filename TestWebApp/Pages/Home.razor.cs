namespace TestWebApp.Pages;

public partial class Home
{
    private List<Block> BlocksList { get; set; } = [];

    private List<Item> ItemsList { get; set; } = [];

    private List<Effect> EffectsList { get; set; } = [];

    private List<Biome> BiomesList { get; set; } = [];

    private List<Entity> EntitiesList { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await InitializeBlocks();
        await InitializeItems();
        await InitializeEffects();
        await InitializeBiomes();
        await InitializeEntities();
    }

    private async Task InitializeItems() =>
        ItemsList = [.. (await ItemRepository.SearchItemsByName("diamond")).OrderBy(item => item.displayName)];

    private async Task InitializeBlocks() =>
        BlocksList = [.. (await BlockRepository.SearchBlocksByName("diamond")).OrderBy(block => block.displayName)];

    private async Task InitializeEffects() =>
        EffectsList = [.. (await EffectRepository.SearchEffectsByName("s")).OrderBy(effect => effect.displayName)];

    private async Task InitializeBiomes() =>
        BiomesList = [.. (await BiomeRepository.SearchBiomesByName("s")).OrderBy(biome => biome.displayName)];

    private async Task InitializeEntities() =>
        EntitiesList = [.. (await EntityRepository.SearchEntitiesByName("s")).OrderBy(entity => entity.displayName)];
}
