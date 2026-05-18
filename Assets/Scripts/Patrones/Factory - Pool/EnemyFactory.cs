using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public static EnemyFactory Instance { get; private set; }

    [Header("Prefabs")]
    [SerializeField] private EnemyMelee _meleePrefab = null;
    [SerializeField] private EnemyRanged _rangedPrefab = null;
    [SerializeField] private PoisonArrow _poisonArrowPrefab = null;
    [SerializeField] private PoisonPool _poisonPoolPrefab = null;

    [Header("Tamaños de pool")]
    [SerializeField] private int _meleePoolSize = 10;
    [SerializeField] private int _rangedPoolSize = 6;
    [SerializeField] private int _arrowPoolSize = 15;
    [SerializeField] private int _poisonPoolSize = 8;

    [Header("Puntos de spawn")]
    [SerializeField] private SpawnPoint[] _spawnPoints = null;
    private int _spawnIndex = 0;

    private ObjectPool<EnemyMelee> _meleePool;
    private ObjectPool<EnemyRanged> _rangedPool;
    private ObjectPool<PoisonArrow> _arrowPool;
    private ObjectPool<PoisonPool> _poisonPool;

    private Transform _playerTransform;
    private Collider _playerCollider;

    private void Awake()
    {
        if (Instance != null && Instance != this) { gameObject.SetActive(false); return; }
        Instance = this;

        _meleePool = new ObjectPool<EnemyMelee>(
            () => Instantiate(_meleePrefab),
            (z, active) => z.gameObject.SetActive(active),
            _meleePoolSize
        );
        _rangedPool = new ObjectPool<EnemyRanged>(
            () => Instantiate(_rangedPrefab),
            (z, active) => z.gameObject.SetActive(active),
            _rangedPoolSize
        );
        _arrowPool = new ObjectPool<PoisonArrow>(
            () => Instantiate(_poisonArrowPrefab),
            (a, active) => a.gameObject.SetActive(active),
            _arrowPoolSize
        );
        _poisonPool = new ObjectPool<PoisonPool>(
            () => Instantiate(_poisonPoolPrefab),
            (p, active) => p.gameObject.SetActive(active),
            _poisonPoolSize
        );
    }

    public void SetPlayerTransform(Transform player)
    {
        _playerTransform = player;
        _playerCollider = player.GetComponent<Collider>();
    }

    private Vector3 GetNextSpawnPosition()
    {
        if (_spawnPoints == null || _spawnPoints.Length == 0)
            return Vector3.zero;

        Vector3 pos = _spawnPoints[_spawnIndex].transform.position;
        _spawnIndex = (_spawnIndex + 1) % _spawnPoints.Length;
        return pos;
    }

    public void ResetSpawnIndex()
    {
        _spawnIndex = 0;
    }

    public EnemyMelee GetMelee(float difficultyMultiplier = 1f)
    {
        var z = _meleePool.GetObject();
        z.transform.position = GetNextSpawnPosition();
        // Pasamos el multiplicador — no creamos objeto nuevo
        z.Initialize(EnemyFlyweightPointer.EnemyMelee, _playerTransform,
                     _playerCollider, difficultyMultiplier);
        return z;
    }

    public EnemyRanged GetRanged(float difficultyMultiplier = 1f)
    {
        var z = _rangedPool.GetObject();
        z.transform.position = GetNextSpawnPosition();
        z.Initialize(EnemyFlyweightPointer.EnemyRanged, _playerTransform,
                     _playerCollider, difficultyMultiplier);
        return z;
    }

    public PoisonArrow GetPoisonArrow()
    {
        return _arrowPool.GetObject();
    }

    public PoisonPool GetPoisonPool()
    {
        return _poisonPool.GetObject();
    }

    public void ReturnMelee(EnemyMelee z) => _meleePool.ReturnObject(z);
    public void ReturnRanged(EnemyRanged z) => _rangedPool.ReturnObject(z);
    public void ReturnPoisonArrow(PoisonArrow a) => _arrowPool.ReturnObject(a);
    public void ReturnPoisonPool(PoisonPool p) => _poisonPool.ReturnObject(p);
}
