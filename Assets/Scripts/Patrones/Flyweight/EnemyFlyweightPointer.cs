public static class EnemyFlyweightPointer
{
    public static readonly EnemyFlyweight EnemyMelee = new EnemyFlyweight
    {
        maxHealth = 100,
        moveSpeed = 2f,
        damage = 10,
        attackRange = 2f,
        detectionRange = 20,
        attackCooldown = 2f
    };

    public static readonly EnemyFlyweight EnemyRanged = new EnemyFlyweight
    {
        maxHealth = 150,
        moveSpeed = 3f,
        damage = 8f,
        attackRange = 15f,
        detectionRange = 25f,
        attackCooldown = 5f,
        arrowSpeed = 12f,
        arrowMaxDistance = 30f,
        retreatDistance = 6f,
        poisonData = new PoisonData
        {
            duration = 5f,
            tickDamage = 5f,
            tickInterval = 0.5f,
            radius = 2.5f
        }
    };
}