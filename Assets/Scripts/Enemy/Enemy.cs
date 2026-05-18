using UnityEngine;

public abstract class Enemy : Entity
{
    protected EnemyFlyweight _data;
    protected Transform _playerTransform;
    protected float _difficultyMultiplier = 1f; 
    private EnemyHealthBar _healthBar;

    public virtual void Initialize(EnemyFlyweight data, Transform playerTransform,
                                   Collider playerCollider, float difficultyMultiplier = 1f)
    {
        _data = data;
        _playerTransform = playerTransform;
        _difficultyMultiplier = difficultyMultiplier;
        _life = data.maxHealth * difficultyMultiplier;

        if (_healthBar == null)
            _healthBar = GetComponentInChildren<EnemyHealthBar>();

        _healthBar?.UpdateHealth(_life, _data.maxHealth * difficultyMultiplier);
        gameObject.SetActive(true);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        _healthBar?.UpdateHealth(_life, _data.maxHealth * _difficultyMultiplier);
    }

    protected virtual void FixedUpdate()
    {
        if (_playerTransform == null || _data == null) return;
        float dist = Vector3.Distance(transform.position, _playerTransform.position);
        HandleBehaviour(dist);
    }

    protected abstract void HandleBehaviour(float distanceToPlayer);

    public override void Die()
    {
        EventManager.ExecuteEvent(GameEvents.Event_EnemyDied, transform.position, this);
        ReturnToPool();
    }

    protected abstract void ReturnToPool();

    protected void FacePlayer()
    {
        Vector3 dir = (_playerTransform.position - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.01f) return;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 8f
        );
    }
}