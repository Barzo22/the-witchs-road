using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
    [SerializeField] private Image _cooldownOverlay;
    [SerializeField] private float _cooldown = 3f; // mismo valor que SecondaryAttack

    private float _remainingCooldown = 0f;
    private bool _onCooldown = false;

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_SecondaryAttackUsed, OnSecondaryAttackUsed);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_SecondaryAttackUsed, OnSecondaryAttackUsed);
    }

    private void OnSecondaryAttackUsed(params object[] parameters)
    {
        if (parameters == null || parameters[0] == null) return;
        _cooldown = (float)parameters[0];
        _remainingCooldown = _cooldown;
        _onCooldown = true;
        _cooldownOverlay.fillAmount = 1f;
    }

    private void Update()
    {
        if (!_onCooldown) return;

        _remainingCooldown -= Time.deltaTime;

        if (_remainingCooldown <= 0f)
        {
            _remainingCooldown = 0f;
            _onCooldown = false;
            _cooldownOverlay.fillAmount = 0f;
            return;
        }

        _cooldownOverlay.fillAmount = _remainingCooldown / _cooldown;
    }
}
