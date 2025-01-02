using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [Header("Collision Settings")]
    [SerializeField] private LayerMask playerLayer; // Layer for detecting the player
    [SerializeField] private LayerMask projectileLayer; // Layer for detecting projectiles
    [SerializeField] private LayerMask environmentLayer; // Layer for environmental collisions

    private HealthComponent healthComponent;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check collision with player
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            HandlePlayerCollision(collision.gameObject);
        }
        // Check collision with projectiles
        else if (((1 << collision.gameObject.layer) & projectileLayer) != 0)
        {
            HandleProjectileCollision(collision.gameObject);
        }
        // Check collision with environment
        else if (((1 << collision.gameObject.layer) & environmentLayer) != 0)
        {
            HandleEnvironmentCollision(collision.gameObject);
        }
    }

    private void HandlePlayerCollision(GameObject player)
    {
        Debug.Log($"{gameObject.name} collided with player: {player.name}");
        
        // Apply damage to the player if the enemy deals contact damage
        var playerHealth = player.GetComponent<HealthComponent>();
        if (playerHealth != null)
        {
            int contactDamage = 5; // Set contact damage
            playerHealth.TakeDamage(contactDamage);
        }
    }

    private void HandleProjectileCollision(GameObject projectile)
    {
        Debug.Log($"{gameObject.name} hit by projectile: {projectile.name}");
        
        // Reduce health if the projectile has a damage component
        var damageComponent = projectile.GetComponent<DamageComponent>();
        if (damageComponent != null && healthComponent != null)
        {
            healthComponent.TakeDamage(damageComponent.GetDamage());
        }

        // Destroy the projectile after impact
        Destroy(projectile);
    }

    private void HandleEnvironmentCollision(GameObject environment)
    {
        Debug.Log($"{gameObject.name} collided with environment: {environment.name}");

        // Handle logic for environmental collision, if necessary
        // For example: Stop movement or play a collision animation
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Handle triggers separately if needed (e.g., detection zones, power-ups)
        Debug.Log($"{gameObject.name} triggered by {collider.name}");
    }
}
