using System.Collections;
using UnityEngine;

public class EnemyMelee : Enemy
{
    private float _attackTimer = 0f;
    private Entity _playerEntity;
    private Rigidbody _rb;
    private Collider _ownCollider;
    private Animator _animator;

    private bool _isDead = false;

    private void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody>();
        _ownCollider = GetComponent<Collider>();
        _animator = GetComponent<Animator>();
    }

    public override void Initialize(
        EnemyFlyweight data,
        Transform playerTransform,
        Collider playerCollider,
        float difficultyMultiplier = 1f)
    {
        base.Initialize(data, playerTransform, playerCollider, difficultyMultiplier);

        _attackTimer = 0f;
        _isDead = false;

        _playerEntity = playerTransform.GetComponent<Entity>();

        if (playerCollider != null && _ownCollider != null)
            Physics.IgnoreCollision(_ownCollider, playerCollider);

        enabled = true;

        if (_animator != null)
        {
            _animator.Rebind();
            _animator.Update(0f);
            _animator.SetBool("IsAttacking", false);
        }
    }

    protected override void HandleBehaviour(float distanceToPlayer)
    {
        if (_isDead)
            return;

        FacePlayer();

        if (distanceToPlayer <= _data.attackRange)
        {
            StopMovement();
            TryAttack();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if (_animator != null)
            _animator.SetBool("IsAttacking", false);

        Vector3 dir = (_playerTransform.position - transform.position).normalized;

        float scaledSpeed = _data.moveSpeed * _difficultyMultiplier;

        _rb.linearVelocity = new Vector3(
            dir.x * scaledSpeed,
            _rb.linearVelocity.y,
            dir.z * scaledSpeed
        );
    }

    private void TryAttack()
    {
        _attackTimer += Time.deltaTime;

        if (_attackTimer < _data.attackCooldown)
            return;

        _attackTimer = 0f;

        _animator.SetTrigger("Attack");

        _playerEntity?.TakeDamage(_data.damage * _difficultyMultiplier);
    }

    private void StopMovement()
    {
        _rb.linearVelocity = new Vector3(0f, _rb.linearVelocity.y, 0f);
    }

    public override void Die()
    {
        if (_isDead)
            return;

        _isDead = true;

        StopMovement();

        enabled = false;

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        if (_animator != null)
            _animator.SetTrigger("Die");

        EventManager.ExecuteEvent(
            GameEvents.Event_EnemyDied,
            transform.position,
            this
        );

        yield return new WaitForSeconds(2f);

        ReturnToPool();
    }

    protected override void ReturnToPool()
    {
        _rb.linearVelocity = Vector3.zero;

        EnemyFactory.Instance.ReturnMelee(this);
    }
}