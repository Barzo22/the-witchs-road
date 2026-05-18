using UnityEngine;

public abstract class Attack : MonoBehaviour, IAttack
{
    [SerializeField] protected float _damage = 10f;

    public virtual void Initialize(float dmg)
    {
        _damage = dmg;
    }

    public float GetDamage() => _damage;

    public abstract void Execute(Vector3 direction);
}