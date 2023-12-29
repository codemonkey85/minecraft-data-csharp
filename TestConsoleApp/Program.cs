foreach (var effect in EffectRepository.SearchEffectsByName("strength")
    .Take(10))
{
    Console.WriteLine($"{effect.id} - {effect.name}");
}
