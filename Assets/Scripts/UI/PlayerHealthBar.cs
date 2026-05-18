using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_PlayerDamaged, OnPlayerDamaged);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_PlayerDamaged, OnPlayerDamaged);
    }

    private void OnPlayerDamaged(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;

        float currentLife = (float)parameters[0];
        float maxLife = (float)parameters[1];

        _fillImage.fillAmount = currentLife / maxLife;
    }
}