namespace TestWebApp.Pages;

public partial class Home
{
    private string LatestPcVersion { get; } = PcVersions.Latest;

    private string LatestBedrockVersion { get; } = BedrockVersions.Latest;

    private List<string> PcVersionsList { get; set; } = [];

    private List<string> BedrockVersionsList { get; set; } = [];

    private List<Block> BlocksList { get; set; } = [];

    private List<Item> ItemsList { get; set; } = [];

    private List<Effect> EffectsList { get; set; } = [];

    private List<Biome> BiomesList { get; set; } = [];

    private List<Entity> EntitiesList { get; set; } = [];

    private List<Enchantment> EnchantmentsList { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        InitializeVersions();
        await InitializeBlocks();
        await InitializeItems();
        await InitializeEffects();
        await InitializeBiomes();
        await InitializeEntities();
        await InitializeEnchantments();
    }

    private void InitializeVersions()
    {
        PcVersionsList = [.. PcVersions.GetAll()];
        BedrockVersionsList = [.. BedrockVersions.GetAll()];
    }

    private async Task InitializeItems() =>
        ItemsList = [.. (await ItemRepository.SearchItemsByName("diamond")).OrderBy(item => item.DisplayName)];

    private async Task InitializeBlocks() =>
        BlocksList = [.. (await BlockRepository.SearchBlocksByName("diamond")).OrderBy(block => block.DisplayName)];

    private async Task InitializeEffects() =>
        EffectsList = [.. (await EffectRepository.SearchEffectsByName("s")).OrderBy(effect => effect.DisplayName)];

    private async Task InitializeBiomes() =>
        BiomesList = [.. (await BiomeRepository.SearchBiomesByName("s")).OrderBy(biome => biome.DisplayName)];

    private async Task InitializeEntities() =>
        EntitiesList = [.. (await EntityRepository.SearchEntitiesByName("s")).OrderBy(entity => entity.DisplayName)];

    private async Task InitializeEnchantments() =>
        EnchantmentsList =
        [
            .. (await EnchantmentRepository.SearchEnchantmentsByName("s")).OrderBy(enchantment =>
                enchantment.DisplayName)
        ];
}
