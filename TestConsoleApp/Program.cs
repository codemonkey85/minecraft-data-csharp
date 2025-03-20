var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((_, services) => services
    .AddSingleton(_ => new DataPathResolver(Constants.DataPathsJson)) // Singleton (shared across the app)
    .AddScoped<MinecraftDataManager>() // Scoped (per request or session)
    .AddScoped<IFileApi, FileApi>()
    .AddScoped<BlockRepository>()
    .AddScoped<EffectRepository>()
    .AddScoped<ItemRepository>()
    .AddScoped<BiomeRepository>()
    .AddScoped<EntityRepository>()
    .AddScoped<EnchantmentRepository>());

using var host = builder.Build();

var minecraftDataManager = host.Services.GetRequiredService<MinecraftDataManager>();
var itemRepository = host.Services.GetRequiredService<ItemRepository>();

minecraftDataManager.SetEdition("pc");
minecraftDataManager.SetVersion("1.16");

var items = await itemRepository.SearchItemsByName("nether");

foreach (var item in items
             .Take(100))
{
    Console.WriteLine($"{item.Id} - {item.Name}");
}
