public static class OrbFlyweightPointer
{
    public static readonly float DropChance = 0.95f;
    public static readonly float HealthOrbChance = 0.15f;
    public static readonly float CommonChance = 0.60f; 
    public static readonly float RareChance = 0.25f; 
                                                     

    public static readonly OrbFlyweight Common = new OrbFlyweight()
    {
        experienceValue = 30f,
        collectRadius = 1.5f,
        glowColor = UnityEngine.Color.white,
        isHealthOrb = false,
        healAmount = 0f
    };

    public static readonly OrbFlyweight Rare = new OrbFlyweight()
    {
        experienceValue = 50f,
        collectRadius = 2f,
        glowColor = UnityEngine.Color.blue,
        isHealthOrb = false,
        healAmount = 0f
    };

    public static readonly OrbFlyweight Epic = new OrbFlyweight()
    {
        experienceValue = 75f,
        collectRadius = 2.5f,
        glowColor = UnityEngine.Color.magenta,
        isHealthOrb = false,
        healAmount = 0f
    };

    public static readonly OrbFlyweight Health = new OrbFlyweight()
    {
        experienceValue = 0f,
        collectRadius = 1.5f,
        glowColor = UnityEngine.Color.green,
        isHealthOrb = true,
        healAmount = 50f
    };
}