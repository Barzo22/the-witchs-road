using UnityEngine;

public class EnemyRanged : Enemy
{
    private float _attackTimer = 0f;
    private Collider _ownCollider = null;
    private Rigidbody _rb;
    private Animator _animator;

    [Header("Configuración del charco")]
    [SerializeField] private float _poolDuration = 5f;
    [SerializeField] private float _poolTickDamage = 5f;
    [SerializeField] private float _poolTickInterval = 0.5f;
    [SerializeField] private float _poolRadius = 2.5f;

    [Header("Configuración de la flecha")]
    [SerializeField] private float _arrowSpeed = 12f;
    [SerializeField] private float _arrowMaxDistance = 30f;
    [SerializeField] private float _retreatDistance = 6f;

    private void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody>();
        _ownCollider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
    }
    public override void Initialize(EnemyFlyweight data, Transform playerTransform,
                                Collider playerCollider, float difficultyMultiplier = 1f)
    {
        base.Initialize(data, playerTransform, playerCollider, difficultyMultiplier);
        _attackTimer = 0f;
    }

    protected override void HandleBehaviour(float distanceToPlayer)
    {
        FacePlayer();

        if (distanceToPlayer < _retreatDistance)
        {
            Retreat();
        }
        else if (distanceToPlayer <= _data.attackRange)
        {
            StopMovement();
            TryShootArrow();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        Move((_playerTransform.position - transform.position).normalized);
    }

    private void Retreat()
    {
        Move((transform.position - _playerTransform.position).normalized);
    }

    private void Move(Vector3 dir)
    {
        _rb.linearVelocity = new Vector3(
            dir.x * _data.moveSpeed,
            _rb.linearVelocity.y,
            dir.z * _data.moveSpeed
        );
    }

    private void StopMovement()
    {
        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
    }

    private void TryShootArrow()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer < _data.attackCooldown)
            return;

        _attackTimer = 0f;

        _animator.SetTrigger("Attack");

        ShootArrow();
    }
    private void ShootArrow()
    {
        PoisonArrow arrow = EnemyFactory.Instance.GetPoisonArrow();
        arrow.transform.position = transform.position + Vector3.up * 0.5f;
        Vector3 dir = (_playerTransform.position + Vector3.up * 0.5f
                       - arrow.transform.position).normalized;

        arrow.Initialize(
            direction: dir,
            speed: _data.arrowSpeed,
            maxDistance: _data.arrowMaxDistance,
            poisonData: _data.poisonData,
            shooterCollider: _ownCollider
        );
    }

    protected override void ReturnToPool()
    {
        _rb.linearVelocity = Vector3.zero;
        EnemyFactory.Instance.ReturnRanged(this);
    }
}
