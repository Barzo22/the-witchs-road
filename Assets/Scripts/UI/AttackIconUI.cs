using UnityEngine;
using UnityEngine.UI;

public class AttackIconUI : MonoBehaviour
{
    [SerializeField] private Image _upgradeArrow;
    [SerializeField] private bool _isPrimary;

    private bool _upgradeAvailable = false;

    private void OnEnable()
    {
        EventManager.SubscribeToEvent(GameEvents.Event_LevelUp, OnLevelUp);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEvents.Event_LevelUp, OnLevelUp);
    }

    private void OnLevelUp(params object[] parameters)
    {
        // Mostramos la flecha cuando sube de nivel
        _upgradeAvailable = true;
        _upgradeArrow.gameObject.SetActive(true);
    }

    public void OnUpgradeSelected()
    {
        if (!_upgradeAvailable) return;
        _upgradeAvailable = false;
        _upgradeArrow.gameObject.SetActive(false);
    }
}