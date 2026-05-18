using UnityEngine;

public class ExperienceManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private float _baseExperience = 100f;
    [SerializeField] private float _scaleMultiplier = 1.3f;
    [SerializeField] private float _upgradeAmount = 5f;

    private float _currentExperience = 0f;
    private int _currentLevel = 1;
    private float _experienceToNextLevel;

    private void Awake()
    {
        _experienceToNextLevel = _baseExperience;
    }

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_OrbCollected, OnOrbCollected);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_OrbCollected, OnOrbCollected);
    }

    private void OnOrbCollected(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;
        float expGained = (float)parameters[0];
        AddExperience(expGained);
    }

    private void AddExperience(float amount)
    {
        _currentExperience += amount;
        EventManager.ExecuteEvent(GameEvents.Event_ExperienceChanged,
            _currentExperience, _experienceToNextLevel, amount);

        if (_currentExperience >= _experienceToNextLevel)
            LevelUp();
    }

    private void LevelUp()
    {
        _currentExperience -= _experienceToNextLevel;
        _currentLevel++;
        _experienceToNextLevel *= _scaleMultiplier;

        EventManager.ExecuteEvent(GameEvents.Event_ExperienceChanged,
            _currentExperience, _experienceToNextLevel, 0f);
        EventManager.ExecuteEvent(GameEvents.Event_LevelUp, _currentLevel);
    }

    public int GetCurrentLevel() => _currentLevel;
    public float GetCurrentExperience() => _currentExperience;
    public float GetExperienceToNextLevel() => _experienceToNextLevel;
}