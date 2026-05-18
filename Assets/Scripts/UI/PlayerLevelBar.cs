using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_ExperienceChanged, OnExperienceChanged);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_ExperienceChanged, OnExperienceChanged);
    }

    private void OnExperienceChanged(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        float currentExp = (float)parameters[0];
        float maxExp = (float)parameters[1];

        _fillImage.fillAmount = currentExp / maxExp;
    }
}