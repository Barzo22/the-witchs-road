using UnityEngine;

public class PoisonArrow : MonoBehaviour
{
    private Vector3 _direction = Vector3.zero;
    private float _speed = 0f;
    private float _maxDistance = 0f;
    private float _distanceTraveled = 0f;
    private bool _hit = false;
    private PoisonData _poisonData;
    private Collider _shooterCollider;
    private Collider _ownCollider;

    private void Awake()
    {
        _ownCollider = GetComponent<Collider>();
    }

    public void Initialize(Vector3 direction, float speed, float maxDistance,
                           PoisonData poisonData, Collider shooterCollider)
    {
        if (_shooterCollider != null && _ownCollider != null)
            Physics.IgnoreCollision(_ownCollider, _shooterCollider, false);

        _direction = direction.normalized;
        _speed = speed;
        _maxDistance = maxDistance;
        _distanceTraveled = 0f;
        _hit = false;
        _poisonData = poisonData;
        _shooterCollider = shooterCollider;

        if (_direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(_direction);

        if (_ownCollider != null && _shooterCollider != null)
            Physics.IgnoreCollision(_ownCollider, _shooterCollider, true);
    }

    private void Update()
    {
        if (_hit) return;

        float step = _speed * Time.deltaTime;
        transform.position += _direction * step;
        _distanceTraveled += step;

        if (_distanceTraveled >= _maxDistance)
            ReturnToPool();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_hit) return;
        _hit = true;

        if (collision.collider.TryGetComponent(out Entity entity) && entity is Player)
            entity.TakeDamage(_poisonData.tickDamage * 2f);

        SpawnPoisonPool();
        ReturnToPool();
    }

    private void SpawnPoisonPool()
    {
        Vector3 spawnPos = transform.position;
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit groundHit, 20f))
            spawnPos = groundHit.point;

        PoisonPool pool = EnemyFactory.Instance.GetPoisonPool();
        pool.transform.position = spawnPos;
        pool.Initialize(_poisonData);
    }

    private void ReturnToPool()
    {
        if (!_hit) SpawnPoisonPool();

        if (_shooterCollider != null && _ownCollider != null)
            Physics.IgnoreCollision(_ownCollider, _shooterCollider, false);

        EnemyFactory.Instance.ReturnPoisonArrow(this);
    }
}
