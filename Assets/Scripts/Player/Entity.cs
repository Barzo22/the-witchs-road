using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    public float maxLife;
    protected float _life;

    protected virtual void Awake()
    {
        _life = maxLife;
    }

    public virtual void TakeDamage(float damage)
    {
        _life -= damage;

        if (_life <= 0)
            Die();
    }

    public abstract void Die();
}