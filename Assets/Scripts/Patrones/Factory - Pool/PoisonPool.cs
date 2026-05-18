using UnityEngine;

public class PoisonPool : MonoBehaviour
{
    private PoisonData _poisonData;
    private float _elapsed = 0f;
    private float _tickTimer = 0f;
    private readonly Collider[] _hitBuffer = new Collider[10];

    public void Initialize(PoisonData poisonData)
    {
        _poisonData = poisonData;
        _elapsed = 0f;
        _tickTimer = 0f;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;
        _tickTimer += Time.deltaTime;

        if (_tickTimer >= _poisonData.tickInterval)
        {
            _tickTimer = 0f;
            DamageInArea();
        }

        if (_elapsed >= _poisonData.duration)
            ReturnToPool();
    }

    private void DamageInArea()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, _poisonData.radius, _hitBuffer);
        for (int i = 0; i < count; i++)
        {
            if (_hitBuffer[i].TryGetComponent(out Entity entity) && entity is Player)
                entity.TakeDamage(_poisonData.tickDamage);
        }
    }

    private void ReturnToPool()
    {
        EnemyFactory.Instance.ReturnPoisonPool(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, _poisonData.radius);
    }
}