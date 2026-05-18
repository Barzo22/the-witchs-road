using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _directDamage = 40f;  
    [SerializeField] private float _areaDamage = 10f;  
    [SerializeField] private float _radius = 2f;   

    [SerializeField] private float _maxDistance = 30f;

    private bool _exploded = false;
    private Rigidbody _rb;
    private Entity _owner;
    private Collider _collider;
    private Vector3 _spawnPosition;

    private Collider[] _hitColliders = new Collider[10];

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        _exploded = false;
    }

    public void Launch(Vector3 direction, float force, Entity owner, Collider playerCollider)
    {
        _owner = owner;
        _spawnPosition = transform.position;
        _rb.linearVelocity = direction * force;
        Physics.IgnoreCollision(_collider, playerCollider);
    }

    private void Update()
    {
        float sqrDistance = (transform.position - _spawnPosition).sqrMagnitude;
        if (sqrDistance >= _maxDistance * _maxDistance)
            Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Entity directHit = collision.collider.GetComponent<Entity>();
        Explode(directHit);
    }

    private void Explode(Entity directHit)
    {
        if (_exploded) return;
        _exploded = true;

        if (directHit != null && directHit != _owner)
            directHit.TakeDamage(_directDamage);

        int hitCount = Physics.OverlapSphereNonAlloc(transform.position, _radius, _hitColliders);

        for (int i = 0; i < hitCount; i++)
        {
            Entity entity = _hitColliders[i].GetComponent<Entity>();

            if (entity != null && entity != _owner && entity != directHit)
                entity.TakeDamage(_areaDamage);
        }

        Destroy(gameObject);
    }
    public void SetDamage(float directDamage, float areaDamage)
    {
        _directDamage = directDamage;
        _areaDamage = areaDamage;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
