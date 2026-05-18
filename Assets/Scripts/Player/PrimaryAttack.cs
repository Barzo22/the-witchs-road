using UnityEngine;

public class PrimaryAttack : Attack
{
    [SerializeField] private float _range = 10f;
    [SerializeField] private LayerMask _targetLayers;
    [SerializeField] private Transform _cameraTransform; 
    private RaycastHit _hit;

    public override void Initialize(float dmg)
    {
        _damage = dmg;
    }

    public override void Execute(Vector3 direction)
    {
        Ray ray = new Ray(_cameraTransform.position, direction);
        Debug.DrawRay(_cameraTransform.position, direction * _range, Color.red);

        if (Physics.Raycast(ray, out _hit, _range, _targetLayers))
        {
            Entity entity = _hit.collider.GetComponent<Entity>();
            if (entity != null)
                entity.TakeDamage(_damage * Time.deltaTime);
        }
    }
}