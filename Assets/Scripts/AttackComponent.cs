using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 2f; // Range within which the enemy can attack
    [SerializeField] private float attackCooldown = 1f; // Time between attacks
    [SerializeField] private int attackDamage = 10; // Damage dealt by each attack
    [SerializeField] private LayerMask targetLayer; // Layer mask to identify attackable targets

    private float lastAttackTime = -Mathf.Infinity; // Tracks the last time an attack was performed
    private Transform attackTarget; // Current target to attack

    private Animator animator; // Reference to the animator

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SetTarget(Transform target)
    {
        attackTarget = target;
    }

    public bool CanAttack()
    {
        if (attackTarget == null) return false;

        float distanceToTarget = Vector2.Distance(transform.position, attackTarget.position);
        return distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown;
    }

    public void PerformAttack()
    {
        if (!CanAttack()) return;

        // Trigger attack animation if available
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Handle actual attack logic (e.g., applying damage to the target)
        ApplyDamage();

        // Reset cooldown timer
        lastAttackTime = Time.time;

        Debug.Log($"{gameObject.name} attacked {attackTarget.name} for {attackDamage} damage.");
    }

    private void ApplyDamage()
    {
        if (attackTarget == null) return;

        // Check if the target has a health component and apply damage
        var healthComponent = attackTarget.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            healthComponent.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw attack range for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
