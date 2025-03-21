var builder = Host.CreateDefaultBuilder(args);

builder.ConfigureServices((_, services) => services
    .AddSingleton<DataPathResolver>() // Singleton (shared across the app)
    .AddScoped<MinecraftDataManager>() // Scoped (per request or session)
    .AddScoped<IFileApi, FileApi>()
    .AddScoped<BlockRepository>()
    .AddScoped<EffectRepository>()
    .AddScoped<ItemRepository>()
    .AddScoped<BiomeRepository>()
    .AddScoped<EntityRepository>()
    .AddScoped<EnchantmentRepository>());

using var host = builder.Build();

var pathResolver = host.Services.GetRequiredService<DataPathResolver>();
await pathResolver.Initialize();

var minecraftDataManager = host.Services.GetRequiredService<MinecraftDataManager>();
var itemRepository = host.Services.GetRequiredService<ItemRepository>();

minecraftDataManager.SetEdition(Editions.Pc);
minecraftDataManager.SetVersion(PcVersions.V1_14);

var items = await itemRepository.SearchItemsByName("nether");

foreach (var item in items
             .Take(100))
{
    Console.WriteLine($"{item.Id} - {item.Name}");
}

Console.WriteLine();

minecraftDataManager.SetVersion(PcVersions.V1_16);

items = await itemRepository.SearchItemsByName("nether");

foreach (var item in items
             .Take(100))
{
    Console.WriteLine($"{item.Id} - {item.Name}");
}
