using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int damageAmount = 10; // Amount of damage dealt
    [SerializeField] private bool destroyOnImpact = true; // Should the object be destroyed after dealing damage
    [SerializeField] private float destroyDelay = 0f; // Delay before destruction (if applicable)

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        HandleCollision(collider.gameObject);
    }

    private void HandleCollision(GameObject target)
    {
        // Check if the target has a HealthComponent
        var healthComponent = target.GetComponent<HealthComponent>();
        if (healthComponent != null)
        {
            // Apply damage to the target
            healthComponent.TakeDamage(damageAmount);

            // Log the damage interaction
            Debug.Log($"{gameObject.name} dealt {damageAmount} damage to {target.name}");

            // Handle destruction if enabled
            if (destroyOnImpact)
            {
                Destroy(gameObject, destroyDelay);
            }
        }
    }

    // Allows external scripts to set damage dynamically
    public void SetDamage(int amount)
    {
        damageAmount = amount;
    }

    // Allows external scripts to retrieve current damage value
    public int GetDamage()
    {
        return damageAmount;
    }
}