using UnityEngine;

public class OrbTest : MonoBehaviour
{
    private bool _isCollected = false;
    private OrbFlyweight _data;
    private Renderer _renderer;

    public enum OrbType { Common, Rare, Epic, Health }
    [SerializeField] private OrbType _orbType;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        ApplyType(_orbType);
    }

    public void SetType(OrbType type)
    {
        _orbType = type;
        ApplyType(type);
    }

    private void ApplyType(OrbType type)
    {
        _data = type switch
        {
            OrbType.Common => OrbFlyweightPointer.Common,
            OrbType.Rare => OrbFlyweightPointer.Rare,
            OrbType.Epic => OrbFlyweightPointer.Epic,
            OrbType.Health => OrbFlyweightPointer.Health,
            _ => OrbFlyweightPointer.Common
        };

        _renderer.material.color = _data.glowColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;
        if (!other.CompareTag("Player")) return;

        Collect();
    }

    public void Collect()
    {
        if (_isCollected) return;
        _isCollected = true;

        if (_data.isHealthOrb)
        {
            // Orbe de vida
            EventManager.ExecuteEvent(GameEvents.Event_HealthOrbCollected, _data.healAmount);
        }
        else
        {
            // Orbe de EXP
            EventManager.ExecuteEvent(GameEvents.Event_OrbCollected,
                _data.experienceValue, transform.position, _data.glowColor);
        }

        OrbFactory.Instance.ReturnOrb(this);
    }

    public static void TurnOnOff(OrbTest orb, bool active)
    {
        if (orb == null) return;
        if (active) orb.ResetOrb();
        orb.gameObject.SetActive(active);
    }

    private void ResetOrb()
    {
        _isCollected = false;
    }
}