using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private AttackIconUI _primaryIcon;
    [SerializeField] private AttackIconUI _secondaryIcon;
    [SerializeField] private float _primaryUpgradeAmount = 5f;
    [SerializeField] private float _secondaryUpgradeAmount = 10f;
    [SerializeField] private TMP_Text _primaryKeyText;
    [SerializeField] private TMP_Text _secondaryKeyText;
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
        _upgradeAvailable = true;

        _primaryKeyText.gameObject.SetActive(true);
        _secondaryKeyText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (!_upgradeAvailable) return;

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            _upgradeAvailable = false;
            EventManager.ExecuteEvent(GameEvents.Event_ApplyUpgrade, true, _primaryUpgradeAmount);
            HideUpgradeArrows();
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            _upgradeAvailable = false;
            EventManager.ExecuteEvent(GameEvents.Event_ApplyUpgrade, false, _secondaryUpgradeAmount);
            HideUpgradeArrows();
        }
    }

    private void HideUpgradeArrows()
    {
        _primaryIcon.OnUpgradeSelected();
        _secondaryIcon.OnUpgradeSelected();

        _primaryKeyText.gameObject.SetActive(false);
        _secondaryKeyText.gameObject.SetActive(false);
    }
}