using UnityEngine;

public class SimpleEnemy : BaseEnemy
{
    protected override void PerformAttack()
    {
        Debug.Log($"{gameObject.name} attacks the player!");
        if (player != null)
        {
            var playerHealth = player.GetComponent<HealthComponent>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // Deal damage to the player.
            }
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        GameManager.Instance.AddScore(10); // Add score for killing the enemy.
    }
}

