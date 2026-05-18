public interface IAttack
{
    void Execute(UnityEngine.Vector3 direction);
    void Initialize(float dmg);
    float GetDamage();
}