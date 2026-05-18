using UnityEngine;

public class SecondaryAttack : Attack
{
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _throwForce = 10f;
    [SerializeField] private float _cooldown = 3f;
    [SerializeField] private float _areaDamageRatio = 0.3f;

    private float _lastUsedTime = -Mathf.Infinity;
    private Collider _playerCollider;
    private Entity _playerEntity;
    private Camera _camera;

    private void Awake()
    {
        _playerCollider = GetComponentInParent<Collider>();
        _playerEntity = GetComponentInParent<Entity>();
        _camera = Camera.main;
    }

    public override void Initialize(float dmg)
    {
        _damage = dmg;
    }

    public override void Execute(Vector3 direction)
    {
        if (Time.time - _lastUsedTime < _cooldown) return;
        _lastUsedTime = Time.time;

        EventManager.ExecuteEvent(GameEvents.Event_SecondaryAttackUsed, _cooldown);

        Ray ray = new Ray(_camera.transform.position, direction);
        Vector3 targetPoint = ray.GetPoint(50f);
        Vector3 bombDirection = (targetPoint - _spawnPoint.position).normalized;
        Bomb bomb = Instantiate(_bombPrefab, _spawnPoint.position, Quaternion.LookRotation(bombDirection));
        bomb.SetDamage(_damage, _damage * _areaDamageRatio);
        bomb.Launch(bombDirection, _throwForce, _playerEntity, _playerCollider);
    }
}