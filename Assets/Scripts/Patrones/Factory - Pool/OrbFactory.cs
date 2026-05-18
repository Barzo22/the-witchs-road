using UnityEngine;

public class OrbFactory : MonoBehaviour
{
    // Singleton para acceso global
    public static OrbFactory Instance;

    [SerializeField] private OrbTest _orbPrefab;
    [SerializeField] private int _initialPoolSize = 10;
    [SerializeField] private bool _dynamic = true;

    private ObjectPool<OrbTest> _orbPool;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            gameObject.SetActive(false);
    }

    private void Start()
    {
        _orbPool = new ObjectPool<OrbTest>(CreateOrb, OrbTest.TurnOnOff, _initialPoolSize, _dynamic);
    }

    private OrbTest CreateOrb()
    {
        return Instantiate(_orbPrefab, transform);
    }

    public OrbTest GetOrb(Vector3 position, OrbTest.OrbType type)
    {
        OrbTest orb = _orbPool.GetObject();
        if (orb == null) return null;

        orb.transform.position = position;
        orb.SetType(type);
        return orb;
    }

    public void ReturnOrb(OrbTest orb)
    {
        _orbPool.ReturnObject(orb);
    }
}
