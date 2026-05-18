using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform = null;
    [SerializeField] private float _scaleMultiplier = 1.3f;
    [SerializeField] private float _timeBetweenWaves = 10f;
    [SerializeField] private float _difficultyMultiplier = 1f;   // dificultad inicial
    [SerializeField] private float _difficultyIncrement = 0.15f; // 15% más difícil por oleada

    private List<WaveData> _waves = new List<WaveData>();
    private int _currentWaveIndex = 0;
    private int _enemiesAlive = 0;
    private WaitForSeconds _spawnWait;
    private WaitForSeconds _waveWait;

    private void Awake()
    {
        // Generamos solo la oleada base — las demás se generan dinámicamente
        WaveData baseWave = new WaveBuilder()
            .SetMelee(4)
            .SetRanged(2)
            .SetSpawnDelay(1.5f)
            .SetTimeBetweenWaves(_timeBetweenWaves)
            .Build();

        _waves.Add(baseWave);
    }

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_EnemyDied, OnEnemyDied);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_EnemyDied, OnEnemyDied);
    }

    private void Start()
    {
        EnemyFactory.Instance.SetPlayerTransform(_playerTransform);
        StartCoroutine(SpawnWave(_waves[_currentWaveIndex]));
    }

    private void OnEnemyDied(params object[] parameters)
    {
        _enemiesAlive--;
        EventManager.ExecuteEvent(GameEvents.Event_EnemyCountChanged, _enemiesAlive);
        if (_enemiesAlive <= 0)
            StartCoroutine(StartNextWave());
    }

    private IEnumerator SpawnWave(WaveData wave)
    {
        EnemyFactory.Instance.ResetSpawnIndex();
        _enemiesAlive = wave.MeleeCount + wave.RangedCount;

        EventManager.ExecuteEvent(GameEvents.Event_WaveStarted,
            _currentWaveIndex + 1, _enemiesAlive);

        _spawnWait = new WaitForSeconds(wave.SpawnDelay);

        for (int i = 0; i < wave.MeleeCount; i++)
        {
            // Pasamos el multiplicador de dificultad actual
            EnemyFactory.Instance.GetMelee(_difficultyMultiplier);
            yield return _spawnWait;
        }

        for (int i = 0; i < wave.RangedCount; i++)
        {
            EnemyFactory.Instance.GetRanged(_difficultyMultiplier);
            yield return _spawnWait;
        }
    }

    private IEnumerator StartNextWave()
    {
        _currentWaveIndex++;

        // Generamos la siguiente oleada escalando desde la anterior
        WaveData nextWave = new WaveBuilder()
            .ScaleFromBase(_waves[_currentWaveIndex - 1], _scaleMultiplier)
            .SetTimeBetweenWaves(_timeBetweenWaves)
            .Build();

        _waves.Add(nextWave);

        // Aumentamos la dificultad para la siguiente oleada
        _difficultyMultiplier += _difficultyIncrement;

        _waveWait = new WaitForSeconds(_timeBetweenWaves);
        yield return _waveWait;

        StartCoroutine(SpawnWave(_waves[_currentWaveIndex]));
    }
}