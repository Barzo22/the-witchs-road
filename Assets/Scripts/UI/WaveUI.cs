using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _waveText;
    [SerializeField] private TextMeshProUGUI _enemiesText;

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_WaveStarted, OnWaveStarted);
        EventManager.SubscribeToEvent(GameEvents.Event_EnemyCountChanged, OnEnemyCountChanged);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_WaveStarted, OnWaveStarted);
        EventManager.Unsubscribe(GameEvents.Event_EnemyCountChanged, OnEnemyCountChanged);
    }

    private void OnWaveStarted(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        int waveNumber = (int)parameters[0];
        int enemyCount = (int)parameters[1];

        _waveText.text = $"Oleada: {waveNumber}";
        _enemiesText.text = $"Enemigos: {enemyCount}";
    }

    private void OnEnemyCountChanged(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        int enemiesAlive = (int)parameters[0];
        _enemiesText.text = $"Enemigos: {enemiesAlive}";
    }
}