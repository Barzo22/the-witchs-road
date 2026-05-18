using UnityEngine;

public class OrbSpawner : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_EnemyDied, OnEnemyDied);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_EnemyDied, OnEnemyDied);
    }

    private void OnEnemyDied(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        if (Random.value > OrbFlyweightPointer.DropChance) return;

        Vector3 spawnPosition =
            (Vector3)parameters[0] + Vector3.up * 0.5f;

        OrbTest.OrbType type = GetRandomOrbType();

        OrbFactory.Instance.GetOrb(spawnPosition, type);
    }
    private OrbTest.OrbType GetRandomOrbType()
    {
        if (Random.value < OrbFlyweightPointer.HealthOrbChance)
            return OrbTest.OrbType.Health;

        float roll = Random.value;

        if (roll < OrbFlyweightPointer.CommonChance)
            return OrbTest.OrbType.Common;
        else if (roll < OrbFlyweightPointer.CommonChance + OrbFlyweightPointer.RareChance)
            return OrbTest.OrbType.Rare;
        else
            return OrbTest.OrbType.Epic;
    }
}