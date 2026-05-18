using UnityEngine;

public class EnemyTest : Entity
{
    public override void Die()
    {
        EventManager.ExecuteEvent(GameEvents.Event_EnemyDied, transform.position);
        gameObject.SetActive(false);
    }
}