using UnityEngine;

public class WaveBuilder
{
    private readonly WaveData _wave = new WaveData();

    public WaveBuilder SetMelee(int count)
    {
        _wave.MeleeCount = count;
        return this;
    }

    public WaveBuilder SetRanged(int count)
    {
        _wave.RangedCount = count;  
        return this;
    }

    public WaveBuilder SetSpawnDelay(float delay)
    {
        _wave.SpawnDelay = delay;
        return this;
    }

    public WaveBuilder SetTimeBetweenWaves(float time)
    {
        _wave.TimeBetweenWaves = time;
        return this;
    }

    public WaveBuilder ScaleFromBase(WaveData baseWave, float multiplier)
    {
        _wave.MeleeCount = Mathf.RoundToInt(baseWave.MeleeCount * multiplier);
        _wave.RangedCount = Mathf.RoundToInt(baseWave.RangedCount * multiplier);
        _wave.SpawnDelay = Mathf.Max(0.3f, baseWave.SpawnDelay * (1f / multiplier));
        return this;
    }

    public WaveData Build() => _wave;
}
