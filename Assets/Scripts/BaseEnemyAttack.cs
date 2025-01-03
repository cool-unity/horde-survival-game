using UnityEngine;

public class BaseEnemyAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 1f;
    private float lastAttackTime;

    public void TryAttack(Transform player)
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack(player);
            lastAttackTime = Time.time;
        }
    }

    private void Attack(Transform player)
    {
        // Example attack logic
        Debug.Log($"{gameObject.name} attacked {player.name}!");
        // Add damage-dealing or animation triggering logic here
    }
}